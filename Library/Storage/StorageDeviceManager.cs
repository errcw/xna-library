using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

namespace Library.Storage
{
    /// <summary>
    /// Defines an action in response to a StorageDeviceEventArgs
    /// </summary>
    public enum StorageDeviceSelectorEventResponse
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        None,

        /// <summary>
        /// Prompt the user to select a new storage device.
        /// </summary>
        Prompt,

        /// <summary>
        /// Force the user to select a new storage device.
        /// </summary>
        Force
    }

    public class StorageDeviceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the desired response to the event.
        /// </summary>
        public StorageDeviceSelectorEventResponse EventResponse { get; set; }
    }

    public class StorageDevicePromptEventArgs : EventArgs
    {
        /// <summary>
        /// Gets whether or not the user has chosen to select a new device.
        /// If true, the StorageDeviceManager will automatically prompt for 
        /// the new device.
        /// </summary>
        public bool PromptForDevice { get; set; }
    }

    /// <summary>
    /// Handles storage devices.
    /// </summary>
    /// <remarks>Adapted from http://www.xnawiki.com/index.php/StorageDeviceManager </remarks>
    public class StorageDeviceManager : GameComponent
    {
        /// <summary>
        /// Fired when a StorageDevice is successfully selected.
        /// </summary>
        public event EventHandler DeviceSelected;

        /// <summary>
        /// Fired when the StorageDevice selector is canceled.
        /// </summary>
        public event EventHandler<StorageDeviceEventArgs> DeviceSelectorCanceled;

        /// <summary>
        /// Fired when the non-forced reselect prompt is closed.
        /// </summary>
        public event EventHandler<StorageDevicePromptEventArgs> DevicePromptClosed;

        /// <summary>
        /// Fired when the StorageDevice becomes disconnected.
        /// </summary>
        public event EventHandler<StorageDeviceEventArgs> DeviceDisconnected;

        /// <summary>
        /// Gets the StorageDevice being managed.
        /// </summary>
        public StorageDevice Device { get; private set; }

        /// <summary>
        /// Gets the player (if any) used for the StorageDevice.
        /// </summary>
        public PlayerIndex? Player { get; private set; }

        /// <summary>
        /// Gets or sets the player to prompt if the storage device is player-agnostic.
        /// </summary>
        public PlayerIndex PlayerToPrompt { get; set; }

        /// <summary>
        /// Gets the amount of space required on the StorageDevice.
        /// </summary>
        public int RequiredBytes { get; private set; }

        /// <summary>
        /// Creates a new player-agnostic StorageDevice with no required amount of free space.
        /// </summary>
        /// <param name="game">The game to which the StorageDeviceManager will be added. The component does not add itself.</param>
        public StorageDeviceManager(Game game)
            : this(game, null, 0)
        {
        }

        /// <summary>
        /// Creates a new player-specific StorageDevice wiht no required amount of free space.
        /// </summary>
        /// <param name="game">The game to which the StorageDeviceManager will be added. The component does not add itself.</param>
        /// <param name="player">The player to prompt for the StorageDevice.</param>
        public StorageDeviceManager(Game game, PlayerIndex player)
            : this(game, player, 0)
        {
        }

        /// <summary>
        /// Creates a new player-agnostic StorageDevice with a required amount of free space.
        /// </summary>
        /// <param name="game">The game to which the StorageDeviceManager will be added. The component does not add itself.</param>
        /// <param name="requiredBytes">The amount of space (in bytes) required.</param>
        public StorageDeviceManager(Game game, int requiredBytes)
            : this(game, null, requiredBytes)
        {
        }

        /// <summary>
        /// Creates a new player-specific StorageDevice with a required amount of free space.
        /// </summary>
        /// <param name="game">The game to which the StorageDeviceManager will be added. The component does not add itself.</param>
        /// <param name="player">The player to prompt for the StorageDevice.</param>
        /// <param name="requiredBytes">The amount of space (in bytes) required.</param>
        public StorageDeviceManager(Game game, PlayerIndex player, int requiredBytes)
            : this(game, (PlayerIndex?)player, requiredBytes)
        {
        }

        private StorageDeviceManager(Game game, PlayerIndex? player, int requiredBytes) : base(game)
        {
            Player = player;
            RequiredBytes = requiredBytes;
            PlayerToPrompt = PlayerIndex.One;
        }

        /// <summary>
        /// Instructs the manager to prompt the user to select a new device.
        /// </summary>
        public void PromptForDevice()
        {
            _showDeviceSelector = true;
        }

        public override void Update(GameTime gameTime)
        {
            // if the device has just become disconnected, fire the event to see if we need to prompt for a new one
            if (Device != null && !Device.IsConnected && _wasDeviceConnected)
                FireDeviceDisconnectedEvent();

            // use a try/catch in case of the following conditions:
            // 1) GamerServicesComponent is not added. In this case Guide.IsVisible throws an exception.
            // 2) Guide.IsVisible returns false but Guide opens (from user input) before the code displays
            //    the Guide. This would cause the Guide to throw an exception.
            try
            {
                // if the Guide is not visible...
                if (!Guide.IsVisible)
                {
                    // if we are to show the device selector...
                    if (_showDeviceSelector)
                    {
                        // don't show device selector next frame; necessary if the user
                        // has only one storage device.
                        _showDeviceSelector = false;

                        // show the selector based on whether we have a player-specific or
                        // player-agnostic storage device.
                        if (Player.HasValue)
                        {
                            Guide.BeginShowStorageDeviceSelector(
                               Player.Value,
                               RequiredBytes,
                               0,
                               DeviceSelectorCallback,
                               null);
                        }
                        else
                        {
                            Guide.BeginShowStorageDeviceSelector(
                               RequiredBytes,
                               0,
                               DeviceSelectorCallback,
                               null);
                        }
                    }

                    // if we are prompting to see if the user wants a new device due to canceling the selector...
                    else if (_promptToReSelectDevice)
                    {
                        Guide.BeginShowMessageBox(
                           (Player.HasValue) ? Player.Value : PlayerToPrompt,
                           "Reselect Storage Device?",
                           ReselectStorageDeviceText,
                           new[] { "Yes. Select new device.", "No. Continue without device." },
                           0,
                           MessageBoxIcon.None,
                           ReselectPromptCallback,
                           null);
                    }

                    // if we are prompting to see if the user wants a new device due to a disconnect...
                    else if (_promptForDisconnect)
                    {
                        Guide.BeginShowMessageBox(
                           (Player.HasValue) ? Player.Value : PlayerToPrompt,
                           "Storage Device Disconnected",
                           DisconnectReselectDeviceText,
                           new[] { "Yes. Select new device.", "No. Continue without device." },
                           0,
                           MessageBoxIcon.None,
                           ReselectPromptCallback,
                           null);
                    }

                    // if we are prompting to force a reselect of the device due to canceling the selector...
                    else if (_promptToForceReselect)
                    {
                        Guide.BeginShowMessageBox(
                           (Player.HasValue) ? Player.Value : PlayerToPrompt,
                           "Reselect Storage Device",
                           ForceReselectDeviceText,
                           new[] { "Ok" },
                           0,
                           MessageBoxIcon.None,
                           ForcePromptCallback,
                           null);
                    }

                    // if we are prompting to force a reselect of the device due to a disconnect...
                    else if (_promptForDisconnectForced)
                    {
                        Guide.BeginShowMessageBox(
                           (Player.HasValue) ? Player.Value : PlayerToPrompt,
                           "Storage Device Disconnected",
                           ForceDisconnectReselectText,
                           new[] { "Ok" },
                           0,
                           MessageBoxIcon.None,
                           ForcePromptCallback,
                           null);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            // store the state of the device's connection
            _wasDeviceConnected = Device != null && Device.IsConnected;
        }

        /// <summary>
        /// The callback used for either of our forced reselect prompts.
        /// </summary>
        /// <param name="ar">The prompt results.</param>
        private void ForcePromptCallback(IAsyncResult ar)
        {
            _promptToForceReselect = false;
            _promptForDisconnectForced = false;

            Guide.EndShowMessageBox(ar);

            _showDeviceSelector = true;
        }

        /// <summary>
        /// The callback used for either of our non-forced reselect prompts.
        /// </summary>
        /// <param name="ar">The prompt results.</param>
        private void ReselectPromptCallback(IAsyncResult ar)
        {
            _promptForDisconnect = false;
            _promptToReSelectDevice = false;

            int? choice = Guide.EndShowMessageBox(ar);
            _showDeviceSelector = choice.HasValue && choice.Value == 0;

            _promptEventArgs.PromptForDevice = _showDeviceSelector;
            if (DevicePromptClosed != null)
            {
                DevicePromptClosed(this, _promptEventArgs);
            }
        }

        /// <summary>
        /// The callback used for the device selector.
        /// </summary>
        /// <param name="ar">The selector results.</param>
        private void DeviceSelectorCallback(IAsyncResult ar)
        {
            Device = Guide.EndShowStorageDeviceSelector(ar);
            if (Device != null)
            {
                if (DeviceSelected != null)
                {
                    DeviceSelected(this, EventArgs.Empty);
                }
            }
            else
            {
                _eventArgs.EventResponse = StorageDeviceSelectorEventResponse.Prompt;
                if (DeviceSelectorCanceled != null)
                {
                    DeviceSelectorCanceled(this, _eventArgs);
                }
                HandleEventArgResults();
            }
        }

        /// <summary>
        /// Fires off the event for a device becoming disconnected and handles the result.
        /// </summary>
        private void FireDeviceDisconnectedEvent()
        {
            _eventArgs.EventResponse = StorageDeviceSelectorEventResponse.Prompt;
            if (DeviceDisconnected != null)
            {
                DeviceDisconnected(this, _eventArgs);
            }
            HandleEventArgResults();
        }

        /// <summary>
        /// Handles the result of the DeviceSelectorCanceled or DeviceDisconnected events.
        /// </summary>
        private void HandleEventArgResults()
        {
            Device = null;

            switch (_eventArgs.EventResponse)
            {
                case StorageDeviceSelectorEventResponse.Prompt:
                    if (_wasDeviceConnected)
                        _promptForDisconnect = true;
                    else
                        _promptToReSelectDevice = true;
                    break;

                case StorageDeviceSelectorEventResponse.Force:
                    if (_wasDeviceConnected)
                        _promptForDisconnectForced = true;
                    else
                        _promptToForceReselect = true;
                    break;

                default:
                    _promptForDisconnect = false;
                    _promptForDisconnectForced = false;
                    _promptToForceReselect = false;
                    _showDeviceSelector = false;
                    break;
            }
        }

        // was the device connected last frame?
        private bool _wasDeviceConnected;

        // should the Guide.BeginShowStorageDeviceSelector be called?
        private bool _showDeviceSelector;

        // should we prompt the user to optionally have them select a new device for canceling the selector?
        private bool _promptToReSelectDevice;

        // should we prompt the user to force them to select a new device for canceling the selector?
        private bool _promptToForceReselect;

        // should we prompt the user to optionally have them select a new device after a device disconnect?
        private bool _promptForDisconnect;

        // should we prompt the user to force them to select a new device after a device disconnect?
        private bool _promptForDisconnectForced;

        private readonly StorageDeviceEventArgs _eventArgs = new StorageDeviceEventArgs();
        private readonly StorageDevicePromptEventArgs _promptEventArgs = new StorageDevicePromptEventArgs();

        private const string ReselectStorageDeviceText =
           "No storage device was selected. Would you like to re-select the storage device?";
        private const string DisconnectReselectDeviceText =
           "An active storage device has been disconnected. Would you like to select a new storage device?";
        private const string ForceReselectDeviceText =
           "No storage device was selected. A storage device is required to continue. Select Ok to choose a storage device.";
        private const string ForceDisconnectReselectText =
           "An active storage device has been disconnected. " +
           "A storage device is required to continue. Select Ok to choose a storage device.";
    }
}