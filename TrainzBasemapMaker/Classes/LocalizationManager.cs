// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TrainzBasemapMaker.Localization;
using TrainzBasemapMaker.Properties;

namespace TrainzBasemapMaker.Classes
{
    public interface ILocalizableForm
    {
        void ApplyLocalization();
    }

    public static class LocalizationManager
    {
        public const string LanguageEnglish = "en";
        public const string LanguagePolish = "pl";

        public static event Action? LanguageChanged;

        public static string CurrentLanguage { get; private set; } = LanguageEnglish;

        public static void Initialize()
        {
            string saved = Settings.Default.Language;
            if (string.IsNullOrWhiteSpace(saved) || (saved != LanguageEnglish && saved != LanguagePolish))
            {
                saved = LanguageEnglish;
                Settings.Default.Language = saved;
                Settings.Default.Save();
            }

            SetLanguage(saved, raiseEvent: false);
        }

        public static void SetLanguage(string langCode, bool raiseEvent = true)
        {
            if (langCode != LanguagePolish && langCode != LanguageEnglish)
            {
                langCode = LanguageEnglish;
            }

            CurrentLanguage = langCode;

            var uiCulture = new CultureInfo(langCode);
            CultureInfo.DefaultThreadCurrentUICulture = uiCulture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
            Strings.Culture = uiCulture;

            // Maintain InvariantCulture for number and coordinate formatting (e.g. 52.123456 with dot separator)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            if (raiseEvent)
            {
                LanguageChanged?.Invoke();
            }
        }

        public static void ApplyLanguageToOpenForms()
        {
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form is ILocalizableForm localizable)
                {
                    localizable.ApplyLocalization();
                }
                form.Invalidate(true);
            }
        }
    }
}
