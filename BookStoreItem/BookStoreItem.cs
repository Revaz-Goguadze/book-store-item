using System.Globalization;

[assembly: CLSCompliant(true)]
#pragma warning disable IDE0079
#pragma warning disable S107

namespace BookStoreItem
{
    /// <summary>
    /// Represents an item in a book store.
    /// </summary>
    public class BookStoreItem
    {
        private readonly string authorName;
        private readonly string? isni;
        private readonly bool hasIsni;
        private decimal price;
        private string? currency;
        private int amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn)
            : this(authorName, title, publisher, isbn, null, string.Empty, 0.00m, "USD", 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="isni"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="isni">A book author's ISNI.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        public BookStoreItem(string authorName, string? isni, string title, string publisher, string isbn)
            : this(authorName, isni, title, publisher, isbn, null, string.Empty, 0.00m, "USD", 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>, <paramref name="published"/>, <paramref name="bookBinding"/>, <paramref name="price"/>, <paramref name="currency"/> and <paramref name="amount"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        /// <param name="published">A book publishing date.</param>
        /// <param name="bookBinding">A book binding type.</param>
        /// <param name="price">An amount of money that a book costs.</param>
        /// <param name="currency">A price currency.</param>
        /// <param name="amount">An amount of books in the store's stock.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn, DateTime? published, string bookBinding, decimal price, string? currency, int amount)
        {
            this.authorName = string.IsNullOrWhiteSpace(authorName) ? throw new ArgumentException("A book author's name can't be empty", nameof(authorName)) : authorName;
            this.Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException($"A book title can't be empty", nameof(title)) : title;
            this.Publisher = string.IsNullOrWhiteSpace(publisher) ? throw new ArgumentException($"A book publisher can't be empty", nameof(publisher)) : publisher;
            this.Isbn = ValidateIsbnFormat(isbn) && ValidateIsbnChecksum(isbn) ? isbn : throw new ArgumentException("Format is invalid", nameof(isbn));
            this.Published = published;
            this.BookBinding = bookBinding;
            this.Price = price;
            this.Currency = currency;
            this.Amount = amount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="isni"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>, <paramref name="published"/>, <paramref name="bookBinding"/>, <paramref name="price"/>, <paramref name="currency"/> and <paramref name="amount"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="isni">A book author's ISNI.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        /// <param name="published">A book publishing date.</param>
        /// <param name="bookBinding">A book binding type.</param>
        /// <param name="price">An amount of money that a book costs.</param>
        /// <param name="currency">A price currency.</param>
        /// <param name="amount">An amount of books in the store's stock.</param>
        public BookStoreItem(string authorName, string? isni, string title, string publisher, string isbn, DateTime? published, string bookBinding, decimal price, string? currency, int amount)
            : this(authorName, title, publisher, isbn, published, bookBinding, price, currency, amount)
        {
            this.isni = ValidateIsni(isni) ? isni : throw new ArgumentException("ISNI is not correct.", nameof(isni));
            this.hasIsni = this.isni != null;
        }

        /// <summary>
        /// Gets a book author's name.
        /// </summary>
        public string AuthorName => this.authorName;

        /// <summary>
        /// Gets an International Standard Name Identifier (ISNI) that uniquely identifies a book author.
        /// </summary>
        public string? Isni => this.isni;

/// <summary>
        /// Gets a value indicating whether an author has an International Standard Name Identifier (ISNI).
        /// </summary>
        public bool HasIsni => this.hasIsni;

/// <summary>
        /// Gets a book title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a book publisher.
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Gets a book International Standard Book Number (ISBN).
        /// </summary>
        public string Isbn { get; private set; }

        /// <summary>
        /// Gets or sets a book publishing date.
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// Gets or sets a book binding type.
        /// </summary>
        public string BookBinding { get; set; }

        /// <summary>
        /// Gets or sets an amount of money that a book costs.
        /// </summary>
        public decimal Price
        {
            get => this.price;
            set => this.price = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a price currency.
        /// </summary>
        public string? Currency
        {
            get => this.currency;
            set
            {
                ThrowExceptionIfCurrencyIsNotValid(value, this.currency);
                this.currency = value;
            }
        }

        /// <summary>
        /// Gets or sets an amount of books in the store's stock.
        /// </summary>
        public int Amount
        {
            get => this.amount;
            set => this.amount = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Gets a <see cref="Uri"/> to the contributor's page at the isni.org website.
        /// </summary>
        /// <returns>A <see cref="Uri"/> to the contributor's page at the isni.org website.</returns>
        public Uri GetIsniUri() => this.HasIsni ? new Uri($"https://isni.org/isni/{this.Isni}") : throw new InvalidOperationException();

        /// <summary>
        /// Gets an <see cref="Uri"/> to the publication page on the isbnsearch.org website.
        /// </summary>
        /// <returns>an <see cref="Uri"/> to the publication page on the isbnsearch.org website.</returns>
        public Uri GetIsbnSearchUri() => new Uri($"https://isbnsearch.org/isbn/{this.Isbn}");

        /// <summary>
        /// Returns the string that represents a current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public new string ToString()
        {
            const int formatBorder = 1000;
            string priceInFormat = this.price > formatBorder ? string.Format(CultureInfo.InvariantCulture, "\"{0:N2} {1}\"", this.price, this.currency) : $"{this.price} {this.currency}";
            return !this.HasIsni ? string.Format(CultureInfo.InvariantCulture, $"{this.Title}, {this.AuthorName}, ISNI IS NOT SET, {priceInFormat}, {this.amount}") : string.Format(CultureInfo.InvariantCulture, $"{this.Title}, {this.AuthorName}, {this.Isni}, {priceInFormat}, {this.amount}");
        }

        private static bool ValidateIsni(string? isni)
        {
            if (string.IsNullOrWhiteSpace(isni))
            {
                return false;
            }

            if (isni.Length != 16)
            {
                return false;
            }

            foreach (var term in isni)
            {
                if (!char.IsLetterOrDigit(term) && term != 'X')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateIsbnFormat(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn) || isbn.Length != 10)
            {
                return false;
            }

            foreach (var term in isbn)
            {
                if (!char.IsDigit(term) && term != 'X')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateIsbnChecksum(string isbn)
        {
            int checkSum = 0;
            int j = 10;
            foreach (var term in isbn)
            {
                checkSum += char.IsDigit(term) ? (term - '0') * j-- : 10;
            }

            return checkSum % 11 == 0;
        }

        private static void ThrowExceptionIfCurrencyIsNotValid(string? currency, string? parameterName)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException($"A {parameterName}'s currency can't be empty or null.", nameof(currency));
            }

            if (currency.Length != 3)
            {
                throw new ArgumentException($"Currency lenght is more than 3.");
            }

            foreach (var t in currency)
            {
                if (!char.IsLetter(t))
                {
                    throw new ArgumentException("Currency is not letters.");
                }
            }
        }
    }
}
