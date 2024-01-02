using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HF_16_StarryNight.Model
{
    class BeeStarModel
    {
        //You can use readonly to create a constant struct value.

        public static readonly Size StarSize = new Size(150, 100);
        private readonly Dictionary<Bee, Point> _bees = new Dictionary<Bee, Point>();
        private readonly Dictionary<Star, Point> _stars = new Dictionary<Star, Point>();
        private Random _random = new Random();

        public BeeStarModel()
        {   //Size.Empty is a value of Size that’s reserved for an empty size.
            //You’ll use it only to create bees and stars when the play area is resized.
            _playAreaSize = Size.Empty;
        }

        //The ViewModel will use a timer to call this Update() method periodically.
        public void Update()
        {
            MoveOneBee();
            AddOrRemoveAStar();
        }

        private static bool RectsOverlap(Rect r1, Rect r2)
        {
            r1.Intersect(r2);
            if (r1.Width > 0 || r1.Height > 0)
                return true;
            return false;
        }
        private Size _playAreaSize;

        public Size PlayAreaSize
        { // Add a backing field, and have the set accessor call CreateBees() and CreateStars()
            get { return _playAreaSize; }
            set
            {
                _playAreaSize = value; // zapomniałeś !!!
                CreateBees();
                CreateStars();
            }
        }

        private void CreateBees()
        { // If the play area is empty => return. +++
            if (PlayAreaSize == Size.Empty)
                return;
            // If there are already bees => move each of them.
            if (_bees.Count() > 0)
            {
                List<Bee> allBees = _bees.Keys.ToList();
                foreach (var bee in allBees)           // in allBees
                    MoveOneBee(bee);
            }

            // Otherwise => create between 5 and 15 randomly sized bees (40 to 150 pixels),
            else
            {
                int beeCount = _random.Next(5, 16);
                for (int i = 0; i < beeCount; i++)
                {
                    int s = _random.Next(40, 151);
                    Size beeSize =
                        new Size(s, s);
                    //+++
                    Point newLocation = FindNonOverlappingPoint(beeSize);
                    // zaimplementować wyszukiwarkę punktów
                    // add it to the _bees collection,
                    Bee newBee = new Bee(newLocation, beeSize);
                    _bees[newBee] = new Point(newLocation.X, newLocation.Y);
                    //do dziennika mozna dodać nowy obikt wskazujac go jako indeks i przypisując nowy klucz
                    //zamiast _bees.Add(new Bee( newLocation, beeSize), newLocation);
                    // and fire the BeeMoved event.
                    OnBeeMoved(newBee, newLocation.X, newLocation.Y);
                }
            }
        }
        private void CreateStars()
        {                       //If the play area is empty, return
            if (PlayAreaSize == Size.Empty)
                return;

            // If there are already stars,
            if (_stars.Count > 0)
            {                   // set each star's location to a new point and fire the StarChanged event,
                foreach (Star star in _stars.Keys)
                {
                    star.Location = FindNonOverlappingPoint(StarSize);
                    OnStarChanged(star, false);
                }
            }
            else                // otherwise call CreateAStar() between 5 and 10 times.
            {
                int starCount = _random.Next(5, 11);
                for (int i = 0; i < starCount; i++)
                    CreateAStar();
            }
        }

        private void CreateAStar()
        {                                   // Find a new non-overlapping point,
            Point newStarLocation =
                FindNonOverlappingPoint(StarSize);
            // add a new Star object to the _stars collection,
            Star newStar = new Star(newStarLocation);
            _stars[newStar] = new Point(newStarLocation.X, newStarLocation.Y);
            // and fire the StarChanged event.
            OnStarChanged(newStar, false);
        }

        private Point FindNonOverlappingPoint(Size size)
        {                               // Find the upper-left corner of a rectangle that doesn't overlap any bees or stars.
            Rect newRect;
            bool noOverlap = false;
            int count = 0;
            // You'll need to try random Rects,
            while (!noOverlap)
            {
                newRect = new Rect(_random.Next((int)PlayAreaSize.Width - 150),
                    _random.Next((int)PlayAreaSize.Height - 150),
                    size.Width, size.Height);
                // then use LINQ queries to find any bees or stars that overlap
                // (the RectsOverlap() method will be useful).
                var overlappingBees =
                    from bee in _bees.Keys
                    where RectsOverlap(bee.Position, newRect)
                    select bee;

                var overlappingStars =
                    from star in _stars.Keys
                    where RectsOverlap(
                        new Rect(star.Location.X, star.Location.Y,
                                  StarSize.Width, StarSize.Height),
                        newRect)
                    select star;

                //If the method’s tried 1,000 random locations and hasn’t found one that doesn’t overlap,
                //the play area has probably run out of space, so just return any point.
                if (overlappingBees.Count() + overlappingStars.Count() == 0 || (count++ > 1000))
                    noOverlap = true;
            }
            return new Point(newRect.X, newRect.Y);
        }

        private void MoveOneBee(Bee bee = null)
        {                           // If there are no bees => return.
            if (_bees.Keys.Count == 0)
                return;
            // If the bee parameter is null =>choose a random bee,
            if (bee == null)
            {
                List<Bee> bees = _bees.Keys.ToList();
                bee = bees[_random.Next(bees.Count)];
            }
            // otherwise => use the bee argument.
            // Then find a new non-overlapping point,
            // update the bee's location,
            bee.Location = FindNonOverlappingPoint(bee.Size);
            // update the _bees collection,
            _bees[bee] = bee.Location;
            // and then fire the OnBeeMoved event.
            OnBeeMoved(bee, bee.Location.X, bee.Location.Y);
        }

        private void AddOrRemoveAStar()
        {
            // Flip a coin (_random.Next(2) == 0)
            // Always create a star if there are <= 5,
            // create a star using CreateAStar()
            if (_random.Next(2) == 0 || //<----- coin flip
                (_stars.Count <= 5 && _stars.Count < 20)) // coś tu mi nie gra !!!! 
                CreateAStar();
            //                 or
            // remove a star and fire OnStarChanged.
            else
            {
                Star starToRemove = _stars.Keys.ToList()[_random.Next(_stars.Count)];
                _stars.Remove(starToRemove);
                OnStarChanged(starToRemove, true);
            }
            // remove one if >= 20.
            // _stars.Keys.ToList()[_random.Next(_stars.Count)] will find a random star.


        }

        // You'll need to add the BeeMoved and StarChanged events and methods to call them.
        // They use the BeeMovedEventArgs and StarChangedEventArgs classes.
        public event EventHandler<BeeMovedEventArgs> BeeMoved;
        protected virtual void OnBeeMoved(Bee beeThatMoved, double x, double y)
        {
            EventHandler<BeeMovedEventArgs> beeMoved = BeeMoved;
            if (beeMoved != null)
            {
                beeMoved(this, new BeeMovedEventArgs(beeThatMoved, x, y)); //musisz utworzyć obiekt argumentów zdarzenia
            }
            //stare wywołanie zdarzenia
        }

        public event EventHandler<StarChangedEventArgs> StarChanged;
        protected virtual void OnStarChanged(Star starThatChanged, bool removed)
        {
            StarChanged?.Invoke(this, new StarChangedEventArgs(starThatChanged, removed));
        }
        //nowe wywołania zdarzenia
    }
}
