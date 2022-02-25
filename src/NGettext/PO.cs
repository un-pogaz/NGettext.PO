using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace NGettext
{
    /// <summary>
    /// <para>Represent a global PO catalog instance.</para>
    /// <para>using static NGettext.Global;</para>
    /// <para>add to PO file for Poedit scan "X-Poedit-KeywordsList: ;_;_p:1,2;_c:1c,2;_cp:1c,2,3\n"</para>
    /// </summary>
    /// <example>
    /// using static NGettext.Global;
    /// </example>
    public static class Global
    {
        /// <summary>
        /// Represents the global PO instance.
        /// </summary>
        public static PO Translator
        {
            get { return _translator ?? Default(); }
            set { _translator = value; }
        }
        static PO _translator;

        /// <summary>
        /// <see cref="NGettext.CustomMoLoader"/> used by the global <see cref="PO"/> translator. Change will reset the Translator.
        /// </summary>
        public static CustomMoLoader MoLoader
        {
            get { return _moLoader ?? (_moLoader = new CustomMoLoader(CustomMoLoader.DefaultDirectory)); }
            set
            {
                _moLoader = value;
                Default();
            }
        }
        static CustomMoLoader _moLoader;


        public static CultureInfo CultureInfo
        {
            get { return _translator?.CultureInfo ?? CultureInfo.CurrentUICulture; }
            set { TranslatorLoad(value);  }
        }

        static PO Default()
        {
            return TranslatorLoad(CultureInfo);
        }

        /// <summary>
        /// Load the Translator to the specified <see cref="System.Globalization.CultureInfo"/>
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static PO TranslatorLoad(CultureInfo cultureInfo)
        {
            return Translator = new PO(MoLoader, cultureInfo);
        }

        /// <summary>
        /// Append a sub-domain to the Translator
        /// </summary>
        /// <param name="domain"></param>
        public static void TranslatorAppendDomain(string domain)
        {
            TranslatorAppendDomain(MoLoader.Directory, domain);
        }
        /// <summary>
        /// Append a sub-domain to the Translator
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="domain"></param>
        public static void TranslatorAppendDomain(string directory, string domain)
        {
            TranslatorAppendMoLoader(new CustomMoLoader(directory, domain, MoLoader.DomainSeparator));
        }
        /// <summary>
        /// Append a sub-domain to the Translator
        /// </summary>
        /// <param name="moStream"></param>
        public static void TranslatorAppendMoLoader(Stream moStream)
        {
            TranslatorAppendMoLoader(new CustomMoLoader(moStream));
        }
        /// <summary>
        /// Append a sub-domain to the Translator
        /// </summary>
        /// <param name="moLoader"></param>
        public static void TranslatorAppendMoLoader(CustomMoLoader moLoader)
        {
            Translator.Load(moLoader);
        }


        /// <summary>
        /// Returns <paramref name="text"/> translated into the selected language.
        /// Similar to <c>gettext</c> function.
        /// </summary>
        /// <param name="text">Text to translate.</param>
        /// <returns>Translated text.</returns>
        public static string _(string text)
        {
            return Translator.GetString(text);
        }
        /// <summary>
        /// Returns <paramref name="text"/> translated into the selected language.
        /// Similar to <c>gettext</c> function.
        /// </summary>
        /// <param name="text">Text to translate.</param>
        /// <param name="args">Optional arguments for <see cref="string.Format(string, object[])"/> method.</param>
        public static string _(string text, params object[] args)
        {
            return Translator.GetString(text, args);
        }

        /// <summary>
        /// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
        /// Similar to <c>pgettext</c> function.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="text">Text to translate.</param>
        /// <returns>Translated text.</returns>
        public static string _c(string context, string text)
        {
            return Translator.GetParticularString(context, text);
        }
        /// <summary>
        /// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
        /// Similar to <c>pgettext</c> function.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="text">Text to translate.</param>
        /// <param name="args">Optional arguments for <see cref="string.Format(string, object[])"/> method.</param>
        /// <returns>Translated text.</returns>
        public static string _c(string context, string text, params object[] args)
        {
            return Translator.GetParticularString(context, text, args);
        }

        /// <summary>
        /// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
        /// Similar to <c>ngettext</c> function.
        /// </summary>
        /// <param name="text">Singular form of message to translate.</param>
        /// <param name="pluralText">Plural form of message to translate.</param>
        /// <param name="n">Value that determines the plural form.</param>
        /// <returns>Translated text.</returns>
        public static string _p(string text, string pluralText, long n)
        {
            return Translator.GetPluralString(text, pluralText, n);
        }
        /// <summary>
        /// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
        /// Similar to <c>ngettext</c> function.
        /// </summary>
        /// <param name="text">Singular form of message to translate.</param>
        /// <param name="pluralText">Plural form of message to translate.</param>
        /// <param name="n">Value that determines the plural form.</param>
        /// <param name="args">Optional arguments for <see cref="string.Format(string, object[])"/> method.</param>
        /// <returns>Translated text.</returns>
        public static string _p(string text, string pluralText, long n, params object[] args)
        {
            return Translator.GetPluralString(text, pluralText, n, args);
        }

        /// <summary>
        /// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
        /// Similar to <c>npgettext</c> function.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="text">Singular form of message to translate.</param>
        /// <param name="pluralText">Plural form of message to translate.</param>
        /// <param name="n">Value that determines the plural form.</param>
        /// <returns>Translated text.</returns>
        public static string _cp(string context, string text, string pluralText, long n)
        {
            return Translator.GetParticularPluralString(context, text, pluralText, n);
        }
        /// <summary>
        /// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
        /// Similar to <c>npgettext</c> function.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="text">Singular form of message to translate.</param>
        /// <param name="pluralText">Plural form of message to translate.</param>
        /// <param name="n">Value that determines the plural form.</param>
        /// <param name="args">Optional arguments for <see cref="string.Format(string, object[])"/> method.</param>
        /// <returns>Translated text.</returns>
        public static string _cp(string context, string text, string pluralText, long n, params object[] args)
        {
            return Translator.GetParticularPluralString(context, text, pluralText, n, args);
        }



        /// <summary>
        /// Get the localized string of a <see cref="Enum"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetLocalizedEnum(this Enum value)
        {
            EnumLocalizerAttribute atrib = GetAttributesFrom<EnumLocalizerAttribute>(value);
            return atrib != null ? atrib.Value : value.ToString();
        }

        static T GetAttributesFrom<T>(Enum value)
        {
            return value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(T), true).OfType<T>().LastOrDefault();
        }

        /// <summary>
        /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class EnumLocalizerAttribute : Attribute
        {
            /// <summary>
            /// Localized string value assosiated to this <see cref="Enum"/> field.
            /// </summary>
            public string Value
            {
                get { return _valueFuction.Invoke(); }
            }
            Func<string> _valueFuction;

            /// <summary>
            /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
            /// Similar to <c>gettext</c> function.
            /// </summary>
            public EnumLocalizerAttribute(string text) : this(() => Translator.GetString(text))
            { }
            /// <summary>
            /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
            /// Similar to <c>pgettext</c> function.
            /// </summary>
            public EnumLocalizerAttribute(string context, string text) : this(() => Translator.GetParticularString(context, text))
            { }
            EnumLocalizerAttribute(Func<string> valueFuction)
            {
                _valueFuction = valueFuction;
            }
        }

        /// <summary>
        /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
        /// Similar to <c>gettext</c> function.
        /// </summary>
        public class _Attribute : EnumLocalizerAttribute
        {
            /// <summary>
            /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
            /// Similar to <c>gettext</c> function.
            /// </summary>
            public _Attribute(string text) : base(text) { }
        }
        /// <summary>
        /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
        /// Similar to <c>pgettext</c> function.
        /// </summary>
        public class _cAttribute : EnumLocalizerAttribute
        {
            /// <summary>
            /// An attribute containing a localized string assosiated to this <see cref="Enum"/> field.
            /// Similar to <c>pgettext</c> function.
            /// </summary>
            public _cAttribute(string context, string text) : base(context, text) { }
        }
    }

    /// <summary>
    /// <para>Represents a PO catalog instance.</para>
    /// </summary>
    public partial class PO : Catalog
    {
        /// <summary>MoLoader of this PO catalog instance</summary>
        public CustomMoLoader MoLoader { get; }

        /// <summary>
        /// Initializes a instance of the <see cref="PO"/> class using given culture info
        /// and loads data for specified directory and Culture info.
        /// </summary>
        /// <param name="directory">Directory that contains PO localization files.</param>
        /// <param name="cultureInfo">Culture info.</param>
        public PO(string directory, CultureInfo cultureInfo) : this(directory, null, cultureInfo)
        { }
        /// <summary>
        /// Initializes a instance of the <see cref="PO"/> class using given culture info
        /// and loads data for specified directory,sub-domain and Culture info.
        /// </summary>
        /// <param name="directory">Directory that contains PO localization files.</param>
        /// <param name="domain">Catalog sub-domain name.</param>
        /// <param name="cultureInfo">Culture info.</param>
        public PO(string directory, string domain, CultureInfo cultureInfo) : this(new CustomMoLoader(directory, domain), cultureInfo)
        { }
        /// <summary>
        /// Initializes a instance of the <see cref="Catalog"/> class using given culture info
        /// and loads data using given loader.
        /// </summary>
        /// <param name="moLoader">Loader instance.</param>
        /// <param name="cultureInfo">Culture info.</param>
        public PO(CustomMoLoader moLoader, CultureInfo cultureInfo) : base(moLoader, cultureInfo)
        {
            MoLoader = moLoader;
        }
    }

    /// <summary>
    /// Custom catalog loader for MO file, based on the Calibre e-book manager.
    /// </summary>
    public class CustomMoLoader : Loaders.MoLoader
    {
        public const string MO_FILE_EXT = ".mo";

        /// <summary>Name for the default directory used in <see cref="CustomMoLoader"/></summary>
        public const string DefaultDirectory = "translations";
        
        /// <summary>Current directory of loader</summary>
        public string Directory { get; }
        /// <summary>Current domain separator of loader</summary>
        public string DomainSeparator { get; }
        /// <summary>Current domain of loader</summary>
        public string Domain { get; }

        /// <summary>
        /// Initializes a instance of the <see cref="CustomMoLoader"/> class which will try to load a MO file
        /// that will be located in the directory and catalog's culture info.
        /// </summary>
        /// <param name="directory"></param>
        public CustomMoLoader(string directory) : this(directory, null)
        { }
        /// <summary>
        /// Initializes a instance of the <see cref="CustomMoLoader"/> class which will try to load a MO file
        /// that will be located in the directory using the sub-domain name and catalog's culture info.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="domain"></param>
        public CustomMoLoader(string directory, string domain) : this(directory, domain, null)
        { }
        /// <summary>
        /// Initializes a instance of the <see cref="CustomMoLoader"/> class which will try to load a MO file
        /// that will be located in the directory using the sub-domain name and catalog's culture info.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="domain"></param>
        /// <param name="domainSeparator"></param>
        public CustomMoLoader(string directory, string domain, string domainSeparator)
            : base(domain = domain ?? string.Empty, directory = string.IsNullOrWhiteSpace(directory) ? DefaultDirectory : directory)
        {
            Directory = directory;
            Domain = domain;
            DomainSeparator = string.IsNullOrWhiteSpace(domainSeparator) ? Path.DirectorySeparatorChar.ToString() : domainSeparator;
        }

        /// <summary>
        /// Initializes a instance of the <see cref="CustomMoLoader"/> class which will try to load a MO file
        /// that will be located in the directory using the sub-domain name and catalog's culture info.
        /// </summary>
        /// <param name="moStream"></param>
        public CustomMoLoader(Stream moStream) : base(moStream)
        {
            Directory = null;
            DomainSeparator = null;
            Domain = null;
        }



        /// <summary>Create a clone of the loader with the specified sub-domain</summary>
        /// <returns></returns>
        public CustomMoLoader Clone(string domain) { return new CustomMoLoader(Directory, domain, DomainSeparator); }

        /// <summary>
        /// Constructs the path to the MO translation file using specified path to the locale directory, 
        /// domain and locale's TwoLetterISOLanguageName string.
        /// </summary>
        /// <param name="localeDir"></param>
        /// <param name="domain"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        protected override string GetFileName(string localeDir, string domain, string locale)
        {
            domain = string.IsNullOrWhiteSpace(domain) ? string.Empty : domain + DomainSeparator;
            return Path.Combine(localeDir, domain + locale + MO_FILE_EXT);
        }



        /// <summary>
        /// Loads translations to the specified catalog using catalog's culture info from specified locale directory and specified domain.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="localeDir"></param>
        /// <param name="catalog"></param>
        protected override void Load(string domain, string localeDir, Catalog catalog)
        {
            var path = FindTranslationFile(catalog.CultureInfo, domain, localeDir);
            if (path != null)
                Load(path, catalog);
        }

        /// <summary>
        /// Loads translations to the specified catalog using specified MO file parser.
        /// </summary>
        /// <param name="parsedMoFile"></param>
        /// <param name="catalog"></param>
        protected override void Load(Loaders.MoFile parsedMoFile, Catalog catalog)
        {
            foreach (var translation in parsedMoFile.Translations)
            {
                if (catalog.Translations.ContainsKey(translation.Key))
                    catalog.Translations[translation.Key] = translation.Value;
                else
                    catalog.Translations.Add(translation.Key, translation.Value);
            }

            if (parsedMoFile.Headers.ContainsKey("Plural-Forms"))
            {
                var generator = this.PluralRuleGenerator as Plural.IPluralRuleTextParser;
                if (generator != null)
                    generator.SetPluralRuleText(parsedMoFile.Headers["Plural-Forms"]);
            }
            catalog.PluralRule = this.PluralRuleGenerator.CreateRule(catalog.CultureInfo);
        }
    }
}
