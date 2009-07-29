﻿using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

using Library.Properties;

namespace Library.Storage
{
    /// <summary>
    /// Event arguments for the Storage class.
    /// </summary>
    public sealed class StorageEventArgs : EventArgs
    {
        /// <summary>
        /// The response to the event. When true, displays a message box
        /// to choose whether to select a new device and shows the selector
        /// if appropriate.
        /// </summary>
    	public bool ShouldPrompt { get; set; }

        /// <summary>
        /// Gets or sets the player index of the controller for which the message
        /// boxes should appear. This does not change the actual selection of the
        /// device but is merely used for the MessageBox displays.
        /// </summary>
    	public PlayerIndex PlayerToPrompt { get; set; }
    }

    /// <summary>
    /// Event arguments for the Storage after a MessageBox prompt has been closed.
    /// </summary>
    public sealed class StoragePromptEventArgs : EventArgs
    {
    	/// <summary>
    	/// Gets whether or not the user has chosen to select a new Storage.
    	/// </summary>
    	public bool ShowDeviceSelector { get; internal set; }
    }

    /// <summary>
    /// A base class for an object that maintains a StorageDevice.
    /// </summary>
    public abstract class Storage : GameComponent
    {
    	/// <summary>
    	/// The name of the StorageContainer used by this Storage.
    	/// </summary>
    	public string StorageContainerName { get; private set; }

    	/// <summary>
    	/// Whether this storage has a valid device.
    	/// </summary>
    	public bool IsValid
    	{
    		get { return _storageDevice != null && _storageDevice.IsConnected; }
    	}

    	/// <summary>
    	/// Occurs when a StorageDevice is selected.
    	/// </summary>
    	public event EventHandler DeviceSelected;

    	/// <summary>
    	/// Occurs when the StorageDevice is disconnected.
    	/// </summary>
    	public event EventHandler<StorageEventArgs> DeviceDisconnected;

    	/// <summary>
    	/// Occurs when a StorageDevice selector is canceled.
    	/// </summary>
    	public event EventHandler<StorageEventArgs> DeviceSelectorCanceled;

    	/// <summary>
    	/// Occurs when the user closes a prompt to reselect a StorageDevice.
    	/// </summary>
    	public event EventHandler<StoragePromptEventArgs> DeviceReselectPromptClosed;


        /// <summary>
        /// Creates a new Storage.
        /// </summary>
        /// <param name=”game”>The current Game instance.</param>
        /// <param name="storageContainerName">The name to use when opening a StorageContainer.</param>
        protected Storage(Game game, string storageContainerName) : base(game)
        {
        	StorageContainerName = storageContainerName;
        } 
    
        /// <summary>
        /// Flags this storage to prompt for a storage device on the next update.
        /// </summary>
        public void PromptForDevice()
        {
            if (_state == StoragePromptState.None)
            {
                _state = StoragePromptState.ShowSelector;
            }
        }

        /// <summary>
        /// Saves a set of IStoredData objects.
        /// </summary>
        /// <param name="saveData">The objects to save.</param>
        public void Save(params IStoreable[] data)
        {
            if (!IsValid)
            {
                throw new InvalidOperationException("StorageDevice is not valid.");
            }

        	using (var container = _storageDevice.OpenContainer(StorageContainerName))
        	{
        		foreach (var datum in data)
        		{
        			var path = Path.Combine(container.Path, datum.FileName);
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        datum.Save(writer.BaseStream);
                    }
        		}
        	}
        }

        /// <summary>
        /// Loads a set of IStoredData objects
        /// </summary>
        /// <param name="saveData">The objects to load.</param>
        public void Load(params IStoreable[] data)
        {
            if (!IsValid)
            {
                throw new InvalidOperationException("StorageDevice is not valid.");
            }

        	using (var container = _storageDevice.OpenContainer(StorageContainerName))
        	{
        		foreach (var datum in data)
        		{
        			var path = Path.Combine(container.Path, datum.FileName);
                    using (StreamReader reader = new StreamReader(path))
                    {
                        datum.Load(reader.BaseStream);
                    }
        		}
        	}
        }

        /// <summary>
        /// Deletes a set of ISaveData object.
        /// </summary>
        /// <param name="saveData">The objects to delete.</param>
        public void Delete(params IStoreable[] data)
        {
            if (!IsValid)
            {
                throw new InvalidOperationException("StorageDevice is not valid.");
            }

            using (var container = _storageDevice.OpenContainer(StorageContainerName))
            {
                foreach (var datum in data)
                {
                    File.Delete(Path.Combine(container.Path, datum.FileName));
                }
            }
        } 

        /// <summary>
        /// Allows the component to update itself.
        /// </summary>
        /// <param name="gameTime">The current game timestamp.</param>
        public override void Update(GameTime gameTime)
        {
        	bool deviceIsConnected = IsValid;

        	if (!deviceIsConnected && _deviceWasConnected)
        	{
        		// if the device was disconnected, fire an event and handle result
                PrepareEventArgs(EventArgs);
                if (DeviceDisconnected != null)
                {
                    DeviceDisconnected(this, EventArgs);
                }
        		HandleEventArgResults();
        	}
        	else if (!deviceIsConnected)
        	{
        		try
        		{
        			if (!Guide.IsVisible)
        			{
        				switch (_state)
        				{
        					case StoragePromptState.ShowSelector:
        						_state = StoragePromptState.None;
        						GetStorageDevice(StorageDeviceSelectorCallback);
        						break;

        					case StoragePromptState.PromptForCanceled:
        						Guide.BeginShowMessageBox(
        							EventArgs.PlayerToPrompt,
        							Resources.StoragePromptReselectTitle,
        							Resources.StoragePromptReselectCancelled,
        							new string[] {
                                        Resources.StoragePromptReselectYes,
                                        Resources.StoragePromptReselectNo },
        							0,
        							MessageBoxIcon.None,
        							ReselectPromptCallback,
        							null);
        						break;

        					case StoragePromptState.PromptForDisconnected:
        						Guide.BeginShowMessageBox(
        							EventArgs.PlayerToPrompt,
        							Resources.StoragePromptReselectTitle,
        							Resources.StoragePromptReselectDisconnected,
        							new string[] {
                                        Resources.StoragePromptReselectYes,
                                        Resources.StoragePromptReselectNo },
        							0,
        							MessageBoxIcon.None,
        							ReselectPromptCallback,
        							null);
        						break;

        					default:
        						break;
        				}
        			}
        		}
        		catch (GuideAlreadyVisibleException)
        		{
        			// swallow the exception
        		}
        	}

        	_deviceWasConnected = deviceIsConnected;
        }

        /// <summary>
        /// Derived classes should implement this method to call the Guide.BeginShowStorageDeviceSelector
        /// method with the desired parameters, using the given callback.
        /// </summary>
        /// <param name="callback">The callback to pass to Guide.BeginShowStorageDeviceSelector.</param>
        protected abstract void GetStorageDevice(AsyncCallback callback);

        /// <summary>
        /// Prepares the StorageEventArgs to be used for an event.
        /// </summary>
        /// <param name="args">The event arguments to be configured.</param>
        protected virtual void PrepareEventArgs(StorageEventArgs args)
        {
        	args.ShouldPrompt = true;
        	args.PlayerToPrompt = PlayerIndex.One;
        }

        /// <summary>
        /// Handles reading from the eventArgs to determine what action to take.
        /// </summary>
        private void HandleEventArgResults()
        {
        	_storageDevice = null;
        	if (EventArgs.ShouldPrompt)
        	{
    			_state = _deviceWasConnected
    	        	? StoragePromptState.PromptForDisconnected
    	        	: StoragePromptState.PromptForCanceled;
            }
            else
            {
    			_state = StoragePromptState.None;
        	}
        }

        private void StorageDeviceSelectorCallback(IAsyncResult result)
        {
        	_storageDevice = Guide.EndShowStorageDeviceSelector(result);
        	if (_storageDevice != null && _storageDevice.IsConnected)
        	{
                if (DeviceSelected != null)
                {
                    DeviceSelected(this, null);
                }
        	}
        	else
        	{
        		PrepareEventArgs(EventArgs);
                if (DeviceSelectorCanceled != null)
                {
                    DeviceSelectorCanceled(this, EventArgs);
                }
        		HandleEventArgResults();
        	}
        }

        private void ReselectPromptCallback(IAsyncResult ar)
        {
        	int? choice = Guide.EndShowMessageBox(ar);

        	// get the device if the user chose the first option
        	_state = choice.HasValue && choice.Value == 0
        		? StoragePromptState.ShowSelector
        		: StoragePromptState.None;

        	// fire an event for the game to know the result of the prompt
        	PromptEventArgs.ShowDeviceSelector = _state == StoragePromptState.ShowSelector;
            if (DeviceReselectPromptClosed != null)
            {
                DeviceReselectPromptClosed(this, PromptEventArgs);
            }
        }

        /// <summary>
        /// The type of prompt to show the user.
        /// </summary>
        internal enum StoragePromptState
        {
        	None, // selector not shown
        	ShowSelector, // need to show the selector
        	PromptForCanceled, // prompt because a selector was canceled
        	PromptForDisconnected, // prompt because a device was disconnected
        }

    	private StorageDevice _storageDevice;
    	private bool _deviceWasConnected;

    	private StoragePromptState _state = StoragePromptState.None;

    	private readonly StorageEventArgs EventArgs = new StorageEventArgs();
    	private readonly StoragePromptEventArgs PromptEventArgs = new StoragePromptEventArgs();
    }
}