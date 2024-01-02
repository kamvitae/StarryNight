using System;
using System.Collections.Generic;
using System.Text;
using HF_16_StarryNight.View;
using HF_16_StarryNight.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Threading;//not good with namespaces :/
using System.Linq;

namespace HF_16_StarryNight.ViewModel
{
    class BeeStarViewModel
    {
        private readonly ObservableCollection<UIElement>
            _sprites = new ObservableCollection<UIElement>();

        public INotifyCollectionChanged Sprites { get => _sprites; }

        private readonly Dictionary<Star, StarControl> _stars =
            new Dictionary<Star, StarControl>();

        private readonly List<StarControl> _fadedStar = new List<StarControl>();

        private BeeStarModel _model = new BeeStarModel();

        private readonly Dictionary<Bee, AnimatedImage> _bees =
            new Dictionary<Bee, AnimatedImage>();

        private DispatcherTimer _timer = new DispatcherTimer();

        public Size PlayAreaSize
        {/* get and set accessors return and set _model.PlayAreaSize */
            get => _model.PlayAreaSize;
            set => _model.PlayAreaSize = value;
        }

        public BeeStarViewModel()
        {   // Hook up the event handlers to the BeeStarModel's:
            // BeeMoved and:
            _model.BeeMoved += BeeMovedHandler;
            // StarChanged events,
            _model.StarChanged += StarChangedHandler;
            // and start the timer ticking every two seconds.
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += timer_tick;
            _timer.Start();
        }

        void timer_tick(object sender, object e)
        {
            // Every time the timer ticks:
            // find all StarControl references in the _fadedStars collection and
            foreach (StarControl starControl in _fadedStar)
                _sprites.Remove(starControl);
            // remove each of them from _sprites, then
            // call the BeeViewModel's Update() method to tell it to update itself.
            _model.Update();
        }

        void BeeMovedHandler(object sender, BeeMovedEventArgs e)
        {// The _bees dictionary maps Bee objects in the Model to AnimatedImage controls
         // in the view.
         // When a bee is moved
         // the BeeViewModel fires its BeeMoved event to tell anyone listening
         // which bee moved and its new location.

            // If the _bees dictionary doesn't already contain an AnimatedImage control for the bee,
            // it needs to create a new one,
            // set its canvas location, and update both _bees and _sprites.
            if (!_bees.ContainsKey(e.BeeThatMoved))
            {
                AnimatedImage beeControl = BeeStarHelper.BeeFactory(
                    e.BeeThatMoved.Width, e.BeeThatMoved.Height, TimeSpan.FromMilliseconds(20));
                BeeStarHelper.SetCanvasLocation(beeControl, e.X, e.Y);
                _bees[e.BeeThatMoved] = beeControl;
                _sprites.Add(beeControl);
            }
            // If the _bees dictionary already has it,
            // then we just need to look up the corresponding AnimatedImage control and
            // move it on the canvas to its new location with an animation.
            else
            {
                AnimatedImage beeControl = _bees[e.BeeThatMoved];
                BeeStarHelper.MoveElementOnCanvas(beeControl, e.X, e.Y);
            }
        }

        void StarChangedHandler(object sender, StarChangedEventArgs e)
        {// The _stars dictionary works just like the _bees one,
         // except that it maps Star objects to their corresponding StarControl controls.
         // The EventArgs contains references to the Star object (which has a Location property) and
         // a Boolean to tell you if the star was removed.

            // If it is then we want it to fade out, so
            // remove it from _stars,
            // add it to _fadedStars, and call its FadeOut() method
            // (it'll be removed from _sprites the next time the Update() method is called,
            // which is why we set the timer’s tick interval to be greater than
            // the StarControl's fade out animation).
            if (e.Removed)
            {
                StarControl starControl = _stars[e.StarThatChanged];
                _stars.Remove(e.StarThatChanged);
                _fadedStar.Add(starControl);
                starControl.FadeOut();
            }
            // If the star is not being removed, then
            // check to see if _stars contains it - if so,
            // get the StarControl reference;
            // if not, you'll need to create a new StarControl,
            // fade it in,
            // add it to _sprites, and
            // send it to back so the bees can fly in front of it.
            else
            {
                StarControl newStar;
                if (_stars.ContainsKey(e.StarThatChanged))
                    newStar = _stars[e.StarThatChanged];
                else
                {
                    newStar = new StarControl();
                    _stars[e.StarThatChanged] = newStar;
                    newStar.FadeIn();
                    BeeStarHelper.SendToBack(newStar);
                    _sprites.Add(newStar);
                }
            // Then set the canvas location for the StarControl.
            /*When you set the new Canvas location, the control is updated—even if it’s already on the Canvas. 
             * This is how the stars move themselves around when the play area is resized.*/
                BeeStarHelper.SetCanvasLocation(newStar,
                    e.StarThatChanged.Location.X, e.StarThatChanged.Location.Y);
            }
        }
    }
}
