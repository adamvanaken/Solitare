namespace Cards
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public enum Suit
    {
        Club = 0, 
        Spade = 1, 
        Heart = 2, 
        Diamond = 3
    }

    public class Card : INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        /// Backing field for the IsFaceUp property.
        /// </summary>
        private bool _isFaceUp;
        
        /// <summary>
        /// Backing field for the IsFaceUp property.
        /// </summary>
        private bool _isEmpty;

        /// <summary>
        /// Backing field for the Rank property.
        /// </summary>
        private int _rank;

        /// <summary>
        /// Backing field for the Suit property.
        /// </summary>
        private Suit _suit;

        /// <summary>
        /// Backing field for the FrontImage property.
        /// </summary>
        private ImageSource frontImage;

        #endregion

        #region Constructors and Destructors

        public Card(Suit suit, int rank, bool isFaceUp, bool isEmpty)
        {
            this.Suit = suit;
            this.Rank = rank;
            this.IsFaceUp = isFaceUp;
            this.IsEmpty = isEmpty;

            // Create an ImageSource that is an image cropped from the
            // whole deck image.
            BitmapImage deckImage =
                new BitmapImage(
                    new Uri("pack://application:,,,/Images/classic-playing-cards.png", UriKind.RelativeOrAbsolute));

            if (isEmpty)
            {
                this.FrontImage = new BitmapImage(
                    new Uri("pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                // We also crop off the rectangular black border so we can have rounded edges on the cards.
                int cellX = (rank - 1) * 73;
                int cellY = (int)suit * 98;
                Int32Rect rect = new Int32Rect(cellX + 4, cellY + 4, 65, 90);
                this.FrontImage = new CroppedBitmap(deckImage, rect);
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event indicating that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the face image of the card.
        /// </summary>
        public ImageSource FrontImage
        {
            get
            {
                return this.frontImage;
            }

            set
            {
                if (!object.Equals(this.frontImage, value))
                {
                    this.frontImage = value;
                    this.RaisePropertyChanged("FrontImage");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the card is face up.
        /// </summary>
        public bool IsFaceUp
        {
            get
            {
                return this._isFaceUp;
            }

            set
            {
                if (!object.Equals(this._isFaceUp, value))
                {
                    this._isFaceUp = value;
                    this.RaisePropertyChanged("IsFaceUp");
                }
            }
        }
        public bool IsEmpty
        {
            get
            {
                return this._isEmpty;
            }

            set
            {
                if (!object.Equals(this._isEmpty, value))
                {
                    this._isEmpty = value;
                    this.RaisePropertyChanged("IsEmpty");
                }
            }
        }

        /// <summary>
        /// Gets or sets the rank of the card.
        /// </summary>
        public int Rank
        {
            get
            {
                return this._rank;
            }

            set
            {
                if (!object.Equals(this._rank, value))
                {
                    this._rank = value;
                    this.RaisePropertyChanged("Rank");
                }
            }
        }

        /// <summary>
        /// Gets or sets the suit of the card.
        /// </summary>
        public Suit Suit
        {
            get
            {
                return this._suit;
            }

            set
            {
                if (!object.Equals(this._suit, value))
                {
                    this._suit = value;
                    this.RaisePropertyChanged("Suit");
                }
            }
        }


        #endregion

        #region Methods

        /// <summary>Raises the PropertyChanged event.</summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}