using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Library.Animation;

namespace Library.Sprite.Descriptor
{
    /// <summary>
    /// A template for creating sprite descriptors. 
    /// </summary>
    public class SpriteDescriptorTemplate
    {
        /// <summary>
        /// Creates a new descriptor template.
        /// </summary>
        /// <param name="descriptorXml">An XML element from the content pipeline describing the sprite.</param>
        public SpriteDescriptorTemplate(XElement descriptorXml)
        {
            _xml = descriptorXml;
        }

        /// <summary>
        /// Creates a descriptor from this template.
        /// </summary>
        /// <param name="content">The content manager to load the descriptor.</param>
        public SpriteDescriptor Create(ContentManager content)
        {
            XElement rootSpriteElem = _xml.Element("Sprites");
            if (rootSpriteElem == null)
            {
                throw new ContentLoadException("Sprite descriptor missing Sprites element");
            }
            _sprites.Clear();
            Sprite root = CreateSprite(rootSpriteElem, content);

            XElement rootAnimElem = _xml.Element("Animations");
            if (rootAnimElem != null)
            {
                _animations.Clear();
                foreach (XElement childAnimElem in rootAnimElem.Elements())
                {
                    CreateAnimation(childAnimElem);
                }
            }

            return new SpriteDescriptor(root,
                new Dictionary<string, Sprite>(_sprites),
                new Dictionary<string, IAnimation>(_animations));
        }

        private Sprite CreateSprite(XElement spriteElem, ContentManager content)
        {
            Sprite sprite = null;
            switch (spriteElem.Name.LocalName)
            {
                case "Sprites": goto case "Composite";
                case "Composite": sprite = CreateCompositeSprite(spriteElem, content); break;
                case "Image": sprite = CreateImageSprite(spriteElem, content); break;
                case "Text": sprite = CreateTextSprite(spriteElem, content); break;
                default: throw new ContentLoadException("Unsupported sprite type " + spriteElem.Name);
            }
            ReadSpriteAttributes(spriteElem, sprite);
            RegisterItem(spriteElem, sprite, _sprites);
            return sprite;
        }

        private Sprite CreateCompositeSprite(XElement spriteElem, ContentManager content)
        {
            CompositeSprite composite = new CompositeSprite();
            foreach (XElement childSpriteElem in spriteElem.Elements())
            {
                composite.Add(CreateSprite(childSpriteElem, content));
            }
            return composite;
        }

        private Sprite CreateImageSprite(XElement spriteElem, ContentManager content)
        {
            string texture = GetAttribute(spriteElem, "Texture");
            return content.Load<ImageSprite>(texture);
        }

        private Sprite CreateTextSprite(XElement spriteElem, ContentManager content)
        {
            string font = GetAttribute(spriteElem, "Font");
            XAttribute textAttr = spriteElem.Attribute("Text");
            string text = (textAttr != null) ? textAttr.Value : String.Empty;
            return new TextSprite(content.Load<SpriteFont>(font), text);
        }

        /// <summary>
        /// Reads a sprite's attributes from the given element into a sprite.
        /// </summary>
        private void ReadSpriteAttributes(XElement spriteElem, Sprite sprite)
        {
            XAttribute positionAttr = spriteElem.Attribute("Position");
            if (positionAttr != null)
            {
                sprite.Position = ParseVector2(positionAttr.Value);
            }
            XAttribute originAttr = spriteElem.Attribute("Origin");
            if (originAttr != null)
            {
                sprite.Origin = ParseVector2(originAttr.Value);
            }
            XAttribute rotationAttr = spriteElem.Attribute("Rotation");
            if (rotationAttr != null)
            {
                sprite.Rotation = ParseFloat(rotationAttr.Value);
            }
            XAttribute scaleAttr = spriteElem.Attribute("Scale");
            if (scaleAttr != null)
            {
                sprite.Origin = ParseVector2(scaleAttr.Value);
            }
            XAttribute colorAttr = spriteElem.Attribute("Color");
            if (colorAttr != null)
            {
                sprite.Color = ParseColor(colorAttr.Value);
            }
            XAttribute layerAttr = spriteElem.Attribute("Layer");
            if (layerAttr != null)
            {
                sprite.Layer = ParseFloat(layerAttr.Value);
            }
        }

        /// <summary>
        /// Creates an animation from a given node. The type of animation to create is
        /// determined by the tag name.
        /// </summary>
        private IAnimation CreateAnimation(XElement animationElem)
        {
            IAnimation animation = null;
            switch (animationElem.Name.LocalName)
            {
                case "Sequential": animation = CreateSequentialAnimation(animationElem); break;
                case "Composite": animation = CreateCompositeAnimation(animationElem); break;
                case "Delay": animation = CreateDelayAnimation(animationElem); break;
                case "Position": animation = CreatePositionAnimation(animationElem); break;
                case "Rotation": animation = CreateRotationAnimation(animationElem); break;
                case "Scale": animation = CreateScaleAnimation(animationElem); break;
                case "Color": animation = CreateColorAnimation(animationElem); break;
                default: throw new ContentLoadException("Unsupported animation type " + animationElem.Name.LocalName);
            }
            RegisterItem(animationElem, animation, _animations);
            return animation;
        }

        private IAnimation CreateSequentialAnimation(XElement animationElem)
        {
            IAnimation[] animations =
                animationElem.Elements().Select(elem => CreateAnimation(elem)).ToArray<IAnimation>();
            SequentialAnimation animation = new SequentialAnimation(animations);
            XAttribute loopAttr = animationElem.Attribute("Loop");
            if (loopAttr != null)
            {
                animation.Loop = ParseBool(loopAttr.Value);
            }
            return animation;
        }

        private IAnimation CreateCompositeAnimation(XElement animationElem)
        {
            IAnimation[] animations =
                animationElem.Elements().Select(elem => CreateAnimation(elem)).ToArray<IAnimation>();
            CompositeAnimation animation = new CompositeAnimation(animations);
            XAttribute loopAttr = animationElem.Attribute("Loop");
            if (loopAttr != null)
            {
                animation.Loop = ParseBool(loopAttr.Value);
            }
            return animation;
        }

        private IAnimation CreateDelayAnimation(XElement animationElem)
        {
            float delay = ParseFloat(GetAttribute(animationElem, "Delay"));
            return new DelayAnimation(delay);
        }

        private IAnimation CreatePositionAnimation(XElement animationElem)
        {
            Sprite sprite = GetNamedSprite(GetAttribute(animationElem, "Sprite"));
            float duration = ParseFloat(GetAttribute(animationElem, "Duration"));
            Vector2 target = ParseVector2(GetAttribute(animationElem, "Target"));
            Ease easing = ParseEasing(GetAttribute(animationElem, "Easing"));
            return new PositionAnimation(sprite, target, duration, Interpolation.InterpolateVector2(easing));
        }

        private IAnimation CreateRotationAnimation(XElement animationElem)
        {
            Sprite sprite = GetNamedSprite(GetAttribute(animationElem, "Sprite"));
            float duration = ParseFloat(GetAttribute(animationElem, "Duration"));
            float target = ParseFloat(GetAttribute(animationElem, "Target"));
            Ease easing = ParseEasing(GetAttribute(animationElem, "Easing"));
            return new RotationAnimation(sprite, target, duration, Interpolation.InterpolateFloat(easing));
        }

        private IAnimation CreateScaleAnimation(XElement animationElem)
        {
            Sprite sprite = GetNamedSprite(GetAttribute(animationElem, "Sprite"));
            float duration = ParseFloat(GetAttribute(animationElem, "Duration"));
            Vector2 target = ParseVector2(GetAttribute(animationElem, "Target"));
            Ease easing = ParseEasing(GetAttribute(animationElem, "Easing"));
            return new ScaleAnimation(sprite, target, duration, Interpolation.InterpolateVector2(easing));
        }

        private IAnimation CreateColorAnimation(XElement animationElem)
        {
            Sprite sprite = GetNamedSprite(GetAttribute(animationElem, "Sprite"));
            float duration = ParseFloat(GetAttribute(animationElem, "Duration"));
            Color target = ParseColor(GetAttribute(animationElem, "Target"));
            Ease easing = ParseEasing(GetAttribute(animationElem, "Easing"));
            return new ColorAnimation(sprite, target, duration, Interpolation.InterpolateColor(easing));
        }

        /// <summary>
        /// Puts a named item into the specified dictionary.
        /// </summary>
        private void RegisterItem<T>(XElement element, T item, Dictionary<String, T> dict)
        {
            XAttribute nameAttr = element.Attribute("Name");
            if (nameAttr != null)
            {
                string name = nameAttr.Value;
                if (dict.ContainsKey(name))
                {
                    throw new ContentLoadException("Descriptor already contains named item " + name);
                }
                dict.Add(name, item);
            }
        }

        /// <summary>
        /// Returns the sprite mapped to the specified name.
        /// </summary>
        /// <param name="name">The name of the sprite to retrieve.</param>
        /// <returns>The sprite</returns>
        /// <exception cref="Microsoft.Xna.Framework.Content.ContentLoadException">
        /// Throw when no sprite with that name exists.</exception>
        private Sprite GetNamedSprite(string name)
        {
            Sprite sprite;
            if (!_sprites.TryGetValue(name, out sprite))
            {
                throw new ContentLoadException("Missing sprite named " + name);
            }
            return sprite;
        }

        /// <summary>
        /// Returns the value of the specified attribute of an element.
        /// </summary>
        /// <param name="element">The element to read the attribute from.</param>
        /// <returns>The value of the attribute.</returns>
        /// <exception cref="Microsoft.Xna.Framework.Content.ContentLoadException">
        /// Throw when the attribute is missing from the element.</exception>
        private string GetAttribute(XElement element, XName attrName)
        {
            XAttribute attr = element.Attribute(attrName);
            if (attr == null)
            {
                throw new ContentLoadException("Missing attribute " + attrName);
            }
            return attr.Value;
        }

        /// <summary>
        /// Returns a float from a string.
        /// </summary>
        private float ParseFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a bool from a string.
        /// </summary>
        private bool ParseBool(string value)
        {
            return bool.Parse(value);
        }

        /// <summary>
        /// Returns a Vector2 from a string of the form "X Y".
        /// </summary>
        private Vector2 ParseVector2(string vec2)
        {
            string[] components = vec2.Split(' ');
            float x = ParseFloat(components[0]);
            float y = ParseFloat(components[1]);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a Color from a string of the form "R G B A".
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Color ParseColor(string color)
        {
            string[] components = color.Split(' ');
            float r = ParseFloat(components[0]);
            float g = ParseFloat(components[1]);
            float b = ParseFloat(components[2]);
            float a = ParseFloat(components[3]);
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Returns an Ease from a string description.
        /// </summary>
        private Ease ParseEasing(string ease)
        {
            switch (ease)
            {
                case "Uniform": return Easing.Uniform;
                case "QuadraticIn": return Easing.QuadraticIn;
                case "QuadraticOut": return Easing.QuadraticOut;
                case "QuadraticInOut": return Easing.QuadraticInOut;
                default: throw new ContentLoadException("Unsupported easing type " + ease);
            }
        }

        private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private Dictionary<string, IAnimation> _animations = new Dictionary<string, IAnimation>();

        private XElement _xml;
    }
}
