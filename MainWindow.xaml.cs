using System.Linq;

namespace Cards
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constants and Fields

        /// <summary>
        /// The DependencyProperty that backs the Stack property.
        /// </summary>
        public static readonly DependencyProperty FlipCardsProperty = DependencyProperty.Register(
            "FlipCards", typeof(ObservableCollection<ObservableCollection<Card>>), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty CardStacksProperty = DependencyProperty.Register(
            "CardStacks", typeof(ObservableCollection<ObservableCollection<Card>>), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HomeStack1Property = DependencyProperty.Register(
            "HomeStack1", typeof(Card), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HomeStack2Property = DependencyProperty.Register(
            "HomeStack2", typeof(Card), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HomeStack3Property = DependencyProperty.Register(
            "HomeStack3", typeof(Card), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HomeStack4Property = DependencyProperty.Register(
            "HomeStack4", typeof(Card), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty TopCardProperty = DependencyProperty.Register(
            "TopCard", typeof(Card), typeof(MainWindow), new FrameworkPropertyMetadata(null));
        
        private Stack<Card> _deck; 
        private Stack<Card> _tempDeck;

        //These keep track of where cards were drawn from so they can be easily replaced
        private int _hitStackX;
        private int _hitStackY;
        private int _topStack;
        private int _numToDeal = 3;
        private bool _pickedLast = false;
        #endregion

        #region Constructors and Destructors

        public MainWindow()
        {
            InitializeComponent();

            CardStack1();
           
        }

        #endregion

        #region Public Properties


        /// <summary>
        /// Gets or sets a stack of cards.
        /// </summary>
        public ObservableCollection<ObservableCollection<Card>> FlipCards
        {
            get
            {
                return (ObservableCollection<ObservableCollection<Card>>)GetValue(FlipCardsProperty);
            }
            set
            {
                SetValue(FlipCardsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a stack of cards.
        /// </summary>
        public ObservableCollection<ObservableCollection<Card>> CardStacks
        {
            get
            {
                return (ObservableCollection<ObservableCollection<Card>>)GetValue(CardStacksProperty);
            }
            set
            {
                SetValue(CardStacksProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the top card
        /// </summary>
        public Card TopCard
        {
            get
            {
                return (Card)GetValue(TopCardProperty);
            }
            set
            {
                SetValue(TopCardProperty, value);
            }
        }
        public Card HomeStack1
        {
            get
            {
                return (Card)GetValue(HomeStack1Property);
            }
            set
            {
                SetValue(HomeStack1Property, value);
            }
        }
        public Card HomeStack2
        {
            get
            {
                return (Card)GetValue(HomeStack2Property);
            }
            set
            {
                SetValue(HomeStack2Property, value);
            }
        }
        public Card HomeStack3
        {
            get
            {
                return (Card)GetValue(HomeStack3Property);
            }
            set
            {
                SetValue(HomeStack3Property, value);
            }
        }
        public Card HomeStack4
        {
            get
            {
                return (Card)GetValue(HomeStack4Property);
            }
            set
            {
                SetValue(HomeStack4Property, value);
            }
        }
        
        #endregion

        #region Methods

        public Stack<Card> Shuffle()
        {
            List<Card> unshuffledDeck = new List<Card>();
            
            // create list of the cards unshuffled.
            for(int i=0; i<52; i++)
            {
                // You can cast an int to an enum.  Unless you explictly specify the enum values,
                // the first enum value is 0, the next is 1, and so forth.
                Suit suit = (Suit)(i / 13);

                // '%' is the modulus operator.  Gives you the remainder of the division.
                int rank = (i % 13) + 1;

                unshuffledDeck.Add(new Card(suit, rank, false, false));
            }

            // randomly shuffle them into a Stack<Card> that will be the deck.
            Stack<Card> deck = new Stack<Card>();
            _tempDeck = new Stack<Card>();
            Random randcard = new Random();
            for(int i=0; i<52; i++)
            {
                // randomly pick a card from what remains in the list.
                int cardIndex = randcard.Next(unshuffledDeck.Count);

                // add it to the deck and remove it from the unshuffled list
                deck.Push(unshuffledDeck[cardIndex]);
                unshuffledDeck.RemoveAt(cardIndex);
            }

            return deck;
        }

        private void CardStack1()
        {
            const int numStacks = 7;
            int cardMax = 1;
            CardStacks = new ObservableCollection<ObservableCollection<Card>>();
            FlipCards = new ObservableCollection<ObservableCollection<Card>>();

            // _deck is the member variable containing the shuffled cards.
            _deck = Shuffle();

            ObservableCollection<Card> stack;
            for (int stackIndex = 0; stackIndex < numStacks; stackIndex++)
            {
                stack = new ObservableCollection<Card>();
                for (int cardIndex = 0; cardIndex < cardMax; cardIndex++)
                {
                    // pop the top card off the shuffled deck and add it to the stack.
                    //  turn it face-up if it's the last card.
                    // (Also contains some logic to avoid putting the important cards in the back
                    //   so the game is more easily winable)
                    if (stackIndex > 4 && cardIndex < 4 && (_deck.Peek().Rank == 1 || _deck.Peek().Rank == 2 || _deck.Peek().Rank == 13))
                    {
                        Card tempCard = _deck.Pop();
                        stack.Add(_deck.Pop());
                        _deck.Push(tempCard);
                    }
                    else
                    {
                        stack.Add(_deck.Pop());
                    }
                    if (cardIndex + 1 == cardMax)
                    {
                        stack[cardIndex].IsFaceUp = true;
                    }
                }
                cardMax++;
                CardStacks.Add(stack);
            }
            
            Suit suit = (Suit)1;
            int rank = 1;

            // initialize cards to be placed in the "empty" stacks
            HomeStack1 = new Card(suit, rank, true, true);
            HomeStack2 = new Card(suit, rank, true, true);
            HomeStack3 = new Card(suit, rank, true, true);
            HomeStack4 = new Card(suit, rank, true, true);
            TopCard = new Card(suit, rank, true, true);

            stack = new ObservableCollection<Card> {TopCard};
            FlipCards.Add(stack);
        }

        private Boolean auto_complete()
        {
            if (_deck.Count == 0 && _tempDeck.Count == 0)
            {
                return CardStacks.All(t => t != null && t.All(t1 => t1.IsFaceUp));
            }
            return false;
        }

        private void AutoComplete()
        {
            // Configure the message box to be displayed
            const string messageBoxText = "Do you want the game \nto auto complete?";
            const string caption = "Auto Complete";
            const MessageBoxButton button = MessageBoxButton.YesNo;
            const MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    CardStacks.Clear();
                    FlipCards.Clear();
                    FlipCards.Add(new ObservableCollection<Card>{new Card((Suit)1, 1, true, true)});
                    if (HomeStack1.IsEmpty || HomeStack2.IsEmpty ||
                        HomeStack3.IsEmpty || HomeStack4.IsEmpty)
                    {
                        HomeStack1 = new Card(Suit.Club, 13, true, false);
                        HomeStack2 = new Card(Suit.Diamond, 13, true, false);
                        HomeStack3 = new Card(Suit.Spade, 13, true, false);
                        HomeStack4 = new Card(Suit.Heart, 13, true, false);
                    }
                    else
                    {
                        HomeStack1 = new Card(HomeStack1.Suit, 13, true, false);
                        HomeStack2 = new Card(HomeStack2.Suit, 13, true, false);
                        HomeStack3 = new Card(HomeStack3.Suit, 13, true, false);
                        HomeStack4 = new Card(HomeStack4.Suit, 13, true, false);
                    }
                    MessageBox.Show("Congratulations, you win! \nGame made by Adam Van Aken");
                    break;
                case MessageBoxResult.No:
                    break;

            }
        }

        private void OnAddCardClicked(object sender, RoutedEventArgs e)
        {
            if (auto_complete())
            {
                AutoComplete();
            }
            else
            {
                // changes card in DeckStack 
                foreach (ObservableCollection<Card> t in FlipCards)
                {
                    if (t[0].IsEmpty == false)
                    {
                        _tempDeck.Push(t[0]);
                    }
                }
                if (_deck.Count > 0)
                {
                    FlipCards.Clear();
                    for (int j = 0; j < _numToDeal; j++)
                    {
                        if (_deck.Count > 0)
                        {
                            FlipCards.Add(new ObservableCollection<Card> {_deck.Pop()});
                            if (FlipCards[j][0].IsEmpty == false)
                            {
                                FlipCards[j][0].IsFaceUp = true;
                            }

                        }
                    }
                }
                else
                {
                    // shows empty stack then resets deck with all cards in the temp stack
                    // adds a temporary card (which is empty and cannot be moved around) 
                    FlipCards.Clear();
                    TopCard = new Card((Suit) 1, 1, true, true);
                    ObservableCollection<Card> stack = new ObservableCollection<Card> { TopCard };
                    FlipCards.Add(stack);

                    int tempCount = _tempDeck.Count;
                    for (int i = 0; i < tempCount; i++)
                    {
                        _deck.Push(_tempDeck.Pop());
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the cards that are being dragged.
        /// </summary>
        public ObservableCollection<Card> DragStack
        {
            get
            {
                return (ObservableCollection<Card>)GetValue(DragStackProperty);
            }
            set
            {
                SetValue(DragStackProperty, value);
            }
        }

        /// <summary>
        /// The DependencyProperty that backs the DragStack property.
        /// </summary>
        public static readonly DependencyProperty DragStackProperty = DependencyProperty.Register(
            "DragStack", typeof(ObservableCollection<Card>), typeof(MainWindow), new FrameworkPropertyMetadata(null));

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (FlipCards.Count > 0)
            {
                TopCard = FlipCards[FlipCards.Count - 1][0];
            }

            Card hitCard = HitTestCard(e.GetPosition(this));
            
            if (hitCard == null)
            {
                base.OnPreviewMouseLeftButtonDown(e);
            }
            else if (hitCard.IsEmpty)
            {
            }
            else
            {
                // stop the left button down event from propagating.
                e.Handled = true;

                // Find where the card was taken from and keep track of it
                _hitStackX = 9;
                _hitStackY = 9;
                DragStack = null;
                for (int x = 0; x < CardStacks.Count; x++)
                {
                    for (int y = 0; y < CardStacks[x].Count; y++)
                    {
                        if (CardStacks[x][y] == hitCard)
                        {
                            _hitStackX = x;
                            _hitStackY = y;
                        }
                    }
                }
                ObservableCollection<Card> cardsToDrag = new ObservableCollection<Card>();
                if (hitCard.IsFaceUp == false && hitCard.IsEmpty == false && _hitStackY == CardStacks[_hitStackX].Count - 1)
                {
                    hitCard.IsFaceUp = true;
                }
                
                else if (TopCard == hitCard && hitCard.IsEmpty == false)
                {
                    _topStack = 0;
                    cardsToDrag.Add(TopCard);
                    if (FlipCards.Count > 0)
                    {
                        //FlipCards[FlipCards.Count - 1] = null;
                        FlipCards.RemoveAt(FlipCards.Count - 1);
                        //replaces the last card in the stack
                        if (_numToDeal > 1 && FlipCards.Count > 0)
                        {
                            TopCard = FlipCards[FlipCards.Count - 1][0];
                        }
                        else if (_numToDeal == 1 && _tempDeck.Count > 0)
                        {
                            FlipCards.Add(new ObservableCollection<Card> {_tempDeck.Pop()});
                        }
                        else if (FlipCards.Count == 0)
                        {
                            if (_tempDeck.Count == 0)
                            {
                                FlipCards.Add(new ObservableCollection<Card> {new Card((Suit) 1, 1, true, true)});
                            }
                            else
                            {
                                _pickedLast = true;
                                FlipCards.Add(new ObservableCollection<Card> {_tempDeck.Pop()});
                            }
                        }
                    }
                    else
                    {
                        //or resets to an "empty" stack
                        TopCard = new Card((Suit)1, 1, false, true);
                        if (TopCard.IsEmpty)
                        {
                            TopCard.IsFaceUp = true;
                        }
                    }                    
                }
                else if (HomeStack1 == hitCard && hitCard.IsEmpty == false)
                {
                    _topStack = 1;
                    cardsToDrag.Add(HomeStack1);
                    if (HomeStack1.Rank > 1)
                    {
                        HomeStack1 = new Card(HomeStack1.Suit, HomeStack1.Rank - 1, true, false);
                    }
                    else
                    {
                        HomeStack1 = new Card((Suit)1, 1, true, true);
                    }
                    
                }
                else if (HomeStack2 == hitCard && hitCard.IsEmpty == false)
                {
                    _topStack = 2;
                    cardsToDrag.Add(HomeStack2);
                    if (HomeStack2.Rank > 1)
                    {
                        HomeStack2 = new Card(HomeStack2.Suit, HomeStack2.Rank - 1, true, false);
                    }
                    else
                    {
                        HomeStack2 = new Card((Suit)1, 1, true, true);
                    }
                    
                }
                else if (HomeStack3 == hitCard && hitCard.IsEmpty == false)
                {
                    _topStack = 3;
                    cardsToDrag.Add(HomeStack3);
                    if (HomeStack3.Rank > 1)
                    {
                        HomeStack3 = new Card(HomeStack3.Suit, HomeStack3.Rank - 1, true, false);
                    }
                    else
                    {
                        HomeStack3 = new Card((Suit)1, 1, true, true);
                    }
                    
                }
                else if (HomeStack4 == hitCard && hitCard.IsEmpty == false)
                {
                    _topStack = 4;
                    cardsToDrag.Add(HomeStack4);
                    if (HomeStack4.Rank > 1)
                    {
                        HomeStack4 = new Card(HomeStack4.Suit, HomeStack4.Rank - 1, true, false);
                    }
                    else
                    {
                        HomeStack4 = new Card((Suit)1, 1, true, true);
                    }
                    
                }
                else
                {
                    DragStack = null;
                    if (hitCard.IsFaceUp && _hitStackX < 9)
                    {
                        for (int z = _hitStackY; z < CardStacks[_hitStackX].Count; z++)
                        {
                            cardsToDrag.Add(CardStacks[_hitStackX][z]);
                        }
                    }
                    else if (_hitStackX < 9 && _hitStackY == (CardStacks[_hitStackX].Count - 1) && hitCard.IsFaceUp == false)
                    {
                        hitCard.IsFaceUp = true;
                    }
                                     
                }
                
                DragStack = cardsToDrag;
                if (_hitStackX != 9)
                {
                    for (int x = 0; x < DragStack.Count; x++)
                    {
                        CardStacks[_hitStackX].Remove(DragStack[x]);
                        if (CardStacks[_hitStackX].Count == 0)
                        {
                            Card emptyCard = new Card(Suit.Club, -4, true, true);
                            CardStacks[_hitStackX].Add(emptyCard);
                        }
                    }

                }
                
                // move the dragItemsControl to where the mouse is.
                Point pnt = e.GetPosition(this);
                Canvas.SetLeft(dragItemsControl, pnt.X - 50);
                Canvas.SetTop(dragItemsControl, pnt.Y - 20);


                // Make the main window capture the mouse so we get all the mouse messages
                // even if they drag outside the window.
                CaptureMouse();
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            // If the DragStack property is not null, we are dragging cards.
            if (DragStack != null)
            {
                e.Handled = true;

                Point pnt = e.GetPosition(this);
                Canvas.SetLeft(dragItemsControl, pnt.X - 50);
                Canvas.SetTop(dragItemsControl, pnt.Y - 20);
            }
            else
            {
                base.OnPreviewMouseMove(e);
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            // If the DragStack property is not null, we are dragging cards
            if (DragStack != null)
            {
                if (DragStack.Count > 0)
                {
                    e.Handled = true;
                    // this card will store the card that gets "placed" on
                    Card placeCard = HitTestCard(e.GetPosition(this));
                    {
                        // stop the left button down event from propagating.
                        e.Handled = true;
                        // if placing a King on an empty stack
                        if (placeCard != null && placeCard.IsEmpty && DragStack[0].Rank == 13)
                        {
                            for (int g = 0; g < 7; g++)
                            {
                                if (CardStacks[g][0] == placeCard)
                                {
                                    if (DragStack == null)
                                    {
                                        break;
                                    }

                                    CardStacks[g].Clear();

                                    foreach (Card t in DragStack)
                                    {
                                        CardStacks[g].Add(t);
                                    }
                                    DragStack = null;
                                }
                            } 
                        }
                        //if (DragStack[0].Rank == 13 && CardStacks.Count > 0)
                        //{
                        //    {
                        //        for (int g = 0; g < 7; g++)
                        //        {
                        //            if (CardStacks[g][0].IsEmpty)
                        //            {
                        //                if (DragStack == null)
                        //                {
                        //                    break;
                        //                }

                        //                CardStacks[g].Clear();

                        //                for (int z = 0; z < DragStack.Count; z++)
                        //                {
                        //                    CardStacks[g].Add(DragStack[z]);
                        //                }
                        //                DragStack = null;
                        //            }
                        //        }
                        //    }
                        //}
                        // or if placing on one of the other stacks
                        else if (HomeStack1 != null && placeCard == HomeStack1 && DragStack.Count != 0)
                        {
                            if (HomeStack1.IsEmpty && DragStack[0].Rank == 1)
                            {
                                HomeStack1 = DragStack[0];
                                DragStack = null;
                            }
                            else if (HomeStack1.Rank == DragStack[0].Rank - 1 && HomeStack1.Suit == DragStack[0].Suit && HomeStack1.IsEmpty == false)
                            {
                                HomeStack1 = DragStack[0];
                                DragStack = null;
                            }
                            CheckWin();

                        }
                        else if (HomeStack2 != null && placeCard == HomeStack2 && DragStack.Count != 0)
                        {
                            if (HomeStack2.IsEmpty && DragStack[0].Rank == 1)
                            {
                                HomeStack2 = DragStack[0];
                                DragStack = null;
                            }
                            else if (HomeStack2.Rank == DragStack[0].Rank - 1 && HomeStack2.Suit == DragStack[0].Suit && HomeStack2.IsEmpty == false)
                            {
                                HomeStack2 = DragStack[0];
                                DragStack = null;
                            }
                            CheckWin();
                        }
                        else if (HomeStack3 != null && placeCard == HomeStack3 && DragStack.Count != 0)
                        {
                            if (HomeStack3.IsEmpty && DragStack[0].Rank == 1)
                            {
                                HomeStack3 = DragStack[0];
                                DragStack = null;
                            }
                            else if (HomeStack3.Rank == DragStack[0].Rank - 1 && HomeStack3.Suit == DragStack[0].Suit && HomeStack3.IsEmpty == false)
                            {
                                HomeStack3 = DragStack[0];
                                DragStack = null;
                            }
                            CheckWin();
                        }
                        else if (HomeStack4 != null && placeCard == HomeStack4 && DragStack.Count != 0)
                        {
                            if (HomeStack4.IsEmpty && DragStack[0].Rank == 1)
                            {
                                HomeStack4 = DragStack[0];
                                DragStack = null;
                            }
                            else if (HomeStack4.Rank == DragStack[0].Rank - 1 && HomeStack4.Suit == DragStack[0].Suit && HomeStack4.IsEmpty == false)
                            {
                                HomeStack4 = DragStack[0];
                                DragStack = null;
                            }
                            CheckWin();
                        }
                        else
                        {
                            // finds where to add the DragStack (if legal move) then adds it
                            for (int x = 0; x < CardStacks.Count; x++)
                            {
                                for (int y = 0; y < CardStacks[x].Count; y++)
                                {
                                    if (CardStacks[x][y] == placeCard)
                                    {
                                        if (DragStack != null && DragStack.Count > 0 &&
                                            (placeCard.Rank - 1 == DragStack[0].Rank) && (y + 1 == CardStacks[x].Count) &&
                                            suitTest(placeCard, DragStack[0]))
                                        {
                                            for (int z = 0; z < DragStack.Count; z++)
                                            {
                                                CardStacks[x].Add(DragStack[z]);
                                            }
                                            DragStack = null;
                                        }
                                    }
                                }
                            }
                        }
                        // replaces DragStack if the user tries to place it on "null" space
                        if (DragStack != null)
                        {
                            if (_hitStackX != 9)
                            {
                                for (int z = 0; z < DragStack.Count; z++)
                                {
                                    if (CardStacks[_hitStackX][0].IsEmpty)
                                    {
                                        CardStacks[_hitStackX].Clear();
                                    }
                                    CardStacks[_hitStackX].Add(DragStack[z]);
                                }
                                DragStack = null;
                            }
                            if (DragStack != null)
                            {
                                if (_topStack == 0)
                                {
                                    if (FlipCards[0][0].IsEmpty)
                                    {
                                        FlipCards.Clear();
                                    }
                                    if ( _numToDeal == 1 && FlipCards.Count > 0)
                                    {
                                        _tempDeck.Push(FlipCards[0][0]);
                                        FlipCards.Clear();
                                    }
                                    if (_pickedLast)
                                    {
                                        _tempDeck.Push(FlipCards[0][0]);
                                        FlipCards.Clear();
                                        _pickedLast = false;
                                    }
                                    FlipCards.Add(new ObservableCollection<Card>{DragStack[0]});
                                    TopCard = DragStack[0];
                                    DragStack = null;
                                }
                                else if (_topStack == 1)
                                {
                                    HomeStack1 = DragStack[0];
                                    DragStack = null;
                                }
                                else if (_topStack == 2)
                                {
                                    HomeStack2 = DragStack[0];
                                    DragStack = null;
                                }
                                else if (_topStack == 3)
                                {
                                    HomeStack3 = DragStack[0];
                                    DragStack = null;
                                }
                                else if (_topStack == 4)
                                {
                                    HomeStack4 = DragStack[0];
                                    DragStack = null;
                                }
                            }
                        }
                    }
                    // just incase, this will always clear the DragStack
                    DragStack = null;
                    //then check for win
                    if (auto_complete())
                    {
                        AutoComplete();
                    }
                }
            }

            else
            {
                base.OnPreviewMouseLeftButtonUp(e);
            }

            // When you are done, release the mouse capture
            ReleaseMouseCapture();
        }
        private void CheckWin()
        {
            if (HomeStack1.Rank == 13 && HomeStack2.Rank == 13 && HomeStack3.Rank == 13 && HomeStack4.Rank == 13)
            {
                MessageBox.Show("Congratulations, you win! \nGame made by Adam Van Aken");
            }
        }

        private Boolean suitTest(Card card1, Card card2)
        {
            if (card2.IsEmpty)
            {
                return false;
            }

            if ((((int)card1.Suit == 0 || (int)card1.Suit == 1) && ((int)card2.Suit == 2 || (int)card2.Suit == 3)) || (((int)card1.Suit == 2 || (int)card1.Suit == 3)&& ((int)card2.Suit == 0 || (int)card2.Suit == 1)))
            {
                return true;
            }
            
            return false;
        }


        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);

            // If you lose the mouse capture for any reason, the drag is canceled.
            // Put the cards back where they came from.

            DragStack = null;
        }

        private Card _hitTestResult;

        private Card HitTestCard(Point point)
        {
            _hitTestResult = null;
            VisualTreeHelper.HitTest(mainGrid, null, OnHitTestResultCallback1, new PointHitTestParameters(point));
            return _hitTestResult;
        }
       
        private HitTestResultBehavior OnHitTestResultCallback1(HitTestResult result)
        {
            FrameworkElement element = result.VisualHit as FrameworkElement;
            if (element != null)
            {
                if (element.Tag is Card)
                {
                    _hitTestResult = (Card)element.Tag;
                    return HitTestResultBehavior.Stop;
                }
            }

            return HitTestResultBehavior.Continue;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DragStack = null;
            _deck.Clear();
            CardStack1();
        }

        private void ToggleDeal(object sender, RoutedEventArgs e)
        {
            _numToDeal = _numToDeal == 3 ? 1 : 3;
            
            DragStack = null;
            _deck.Clear();
            CardStack1();
        }
    }
}