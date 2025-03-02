﻿using API.BusinessLogic;
using API.Models.DTOs.Other;
using API.Services.Other;

namespace API.Seeders
{
    public class CountrySeeder
    {
        private readonly CountriesService _countriesService;

        public CountrySeeder(CountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public static async Task SeedAsync(CountriesService countriesService)
        {
            var existingCountries = await countriesService.GetAllAsync();
            if (existingCountries != null && existingCountries.TotalItemCount > 0)
            {
                return; // Data is already seeded
            }

            var countries = new List<CountryDto>
            {
                new CountryDto { Name = "Poland", FullName = "Republic of Poland", Abbreviation = "PL", DialingCode = "+48" },
                new CountryDto { Name = "Afghanistan", FullName = "Afghanistan", Abbreviation = "AF", DialingCode = "+93" },
                new CountryDto { Name = "Albania", FullName = "Albania", Abbreviation = "AL", DialingCode = "+355" },
                new CountryDto { Name = "Algeria", FullName = "People's Democratic Republic of Algeria", Abbreviation = "DZ", DialingCode = "+213" },
                new CountryDto { Name = "Andorra", FullName = "Principality of Andorra", Abbreviation = "AD", DialingCode = "+376" },
                new CountryDto { Name = "Angola", FullName = "Angola", Abbreviation = "AO", DialingCode = "+244" },
                new CountryDto { Name = "Antigua and Barbuda", FullName = "Antigua and Barbuda", Abbreviation = "AG", DialingCode = "+1-268" },
                new CountryDto { Name = "Argentina", FullName = "Argentine Republic", Abbreviation = "AR", DialingCode = "+54" },
                new CountryDto { Name = "Armenia", FullName = "Republic of Armenia", Abbreviation = "AM", DialingCode = "+374" },
                new CountryDto { Name = "Australia", FullName = "Australia", Abbreviation = "AU", DialingCode = "+61" },
                new CountryDto { Name = "Austria", FullName = "Republic of Austria", Abbreviation = "AT", DialingCode = "+43" },
                new CountryDto { Name = "Azerbaijan", FullName = "Azerbaijan Republic", Abbreviation = "AZ", DialingCode = "+994" },
                new CountryDto { Name = "Bahamas", FullName = "Commonwealth of The Bahamas", Abbreviation = "BS", DialingCode = "+1-242" },
                new CountryDto { Name = "Bahrain", FullName = "Bahrain", Abbreviation = "BH", DialingCode = "+973" },
                new CountryDto { Name = "Bangladesh", FullName = "People's Republic of Bangladesh", Abbreviation = "BD", DialingCode = "+880" },
                new CountryDto { Name = "Barbados", FullName = "Barbados", Abbreviation = "BB", DialingCode = "+1-246" },
                new CountryDto { Name = "Belarus", FullName = "Republic of Belarus", Abbreviation = "BY", DialingCode = "+375" },
                new CountryDto { Name = "Belgium", FullName = "Kingdom of Belgium", Abbreviation = "BE", DialingCode = "+32" },
                new CountryDto { Name = "Belize", FullName = "Belize", Abbreviation = "BZ", DialingCode = "+501" },
                new CountryDto { Name = "Benin", FullName = "Republic of Benin", Abbreviation = "BJ", DialingCode = "+229" },
                new CountryDto { Name = "Bhutan", FullName = "Kingdom of Bhutan", Abbreviation = "BT", DialingCode = "+975" },
                new CountryDto { Name = "Bolivia", FullName = "Plurinational State of Bolivia", Abbreviation = "BO", DialingCode = "+591" },
                new CountryDto { Name = "Bosnia and Herzegovina", FullName = "Bosnia and Herzegovina", Abbreviation = "BA", DialingCode = "+387" },
                new CountryDto { Name = "Botswana", FullName = "Republic of Botswana", Abbreviation = "BW", DialingCode = "+267" },
                new CountryDto { Name = "Brazil", FullName = "Federative Republic of Brazil", Abbreviation = "BR", DialingCode = "+55" },
                new CountryDto { Name = "Brunei", FullName = "Brunei Darussalam", Abbreviation = "BN", DialingCode = "+673" },
                new CountryDto { Name = "Bulgaria", FullName = "Republic of Bulgaria", Abbreviation = "BG", DialingCode = "+359" },
                new CountryDto { Name = "Burkina Faso", FullName = "Burkina Faso", Abbreviation = "BF", DialingCode = "+226" },
                new CountryDto { Name = "Burundi", FullName = "Republic of Burundi", Abbreviation = "BI", DialingCode = "+257" },
                new CountryDto { Name = "Cabo Verde", FullName = "Cabo Verde", Abbreviation = "CV", DialingCode = "+238" },
                new CountryDto { Name = "Cambodia", FullName = "Kingdom of Cambodia", Abbreviation = "KH", DialingCode = "+855" },
                new CountryDto { Name = "Cameroon", FullName = "Republic of Cameroon", Abbreviation = "CM", DialingCode = "+237" },
                new CountryDto { Name = "Canada", FullName = "Canada", Abbreviation = "CA", DialingCode = "+1" },
                new CountryDto { Name = "Central African Republic", FullName = "Central African Republic", Abbreviation = "CF", DialingCode = "+236" },
                new CountryDto { Name = "Chad", FullName = "Chad", Abbreviation = "TD", DialingCode = "+235" },
                new CountryDto { Name = "Chile", FullName = "Republic of Chile", Abbreviation = "CL", DialingCode = "+56" },
                new CountryDto { Name = "China", FullName = "People's Republic of China", Abbreviation = "CN", DialingCode = "+86" },
                new CountryDto { Name = "Colombia", FullName = "Republic of Colombia", Abbreviation = "CO", DialingCode = "+57" },
                new CountryDto { Name = "Comoros", FullName = "Union of the Comoros", Abbreviation = "KM", DialingCode = "+269" },
                new CountryDto { Name = "Congo", FullName = "Republic of the Congo", Abbreviation = "CG", DialingCode = "+242" },
                new CountryDto { Name = "Congo (Democratic Republic)", FullName = "Democratic Republic of the Congo", Abbreviation = "CD", DialingCode = "+243" },
                new CountryDto { Name = "Costa Rica", FullName = "Republic of Costa Rica", Abbreviation = "CR", DialingCode = "+506" },
                new CountryDto { Name = "Croatia", FullName = "Republic of Croatia", Abbreviation = "HR", DialingCode = "+385" },
                new CountryDto { Name = "Cuba", FullName = "Cuba", Abbreviation = "CU", DialingCode = "+53" },
                new CountryDto { Name = "Cyprus", FullName = "Cyprus", Abbreviation = "CY", DialingCode = "+357" },
                new CountryDto { Name = "Czech Republic", FullName = "Czech Republic", Abbreviation = "CZ", DialingCode = "+420" },
                new CountryDto { Name = "Denmark", FullName = "Kingdom of Denmark", Abbreviation = "DK", DialingCode = "+45" },
                new CountryDto { Name = "Djibouti", FullName = "Djibouti", Abbreviation = "DJ", DialingCode = "+253" },
                new CountryDto { Name = "Dominica", FullName = "Commonwealth of Dominica", Abbreviation = "DM", DialingCode = "+1-767" },
                new CountryDto { Name = "Dominican Republic", FullName = "Dominican Republic", Abbreviation = "DO", DialingCode = "+1-809, 1-829, 1-849" },
                new CountryDto { Name = "Ecuador", FullName = "Republic of Ecuador", Abbreviation = "EC", DialingCode = "+593" },
                new CountryDto { Name = "Egypt", FullName = "Arab Republic of Egypt", Abbreviation = "EG", DialingCode = "+20" },
                new CountryDto { Name = "El Salvador", FullName = "El Salvador", Abbreviation = "SV", DialingCode = "+503" },
                new CountryDto { Name = "Equatorial Guinea", FullName = "Republic of Equatorial Guinea", Abbreviation = "GQ", DialingCode = "+240" },
                new CountryDto { Name = "Eritrea", FullName = "State of Eritrea", Abbreviation = "ER", DialingCode = "+291" },
                new CountryDto { Name = "Estonia", FullName = "Estonia", Abbreviation = "EE", DialingCode = "+372" },
                new CountryDto { Name = "Eswatini", FullName = "Kingdom of Eswatini", Abbreviation = "SZ", DialingCode = "+268" },
                new CountryDto { Name = "Ethiopia", FullName = "Federal Democratic Republic of Ethiopia", Abbreviation = "ET", DialingCode = "+251" },
                new CountryDto { Name = "Fiji", FullName = "Republic of Fiji", Abbreviation = "FJ", DialingCode = "+679" },
                new CountryDto { Name = "Finland", FullName = "Republic of Finland", Abbreviation = "FI", DialingCode = "+358" },
                new CountryDto { Name = "France", FullName = "French Republic", Abbreviation = "FR", DialingCode = "+33" },
                new CountryDto { Name = "Gabon", FullName = "Gabonese Republic", Abbreviation = "GA", DialingCode = "+241" },
                new CountryDto { Name = "Gambia", FullName = "Republic of The Gambia", Abbreviation = "GM", DialingCode = "+220" },
                new CountryDto { Name = "Georgia", FullName = "Georgia", Abbreviation = "GE", DialingCode = "+995" },
                new CountryDto { Name = "Germany", FullName = "Federal Republic of Germany", Abbreviation = "DE", DialingCode = "+49" },
                new CountryDto { Name = "Ghana", FullName = "Ghana", Abbreviation = "GH", DialingCode = "+233" },
                new CountryDto { Name = "Greece", FullName = "Hellenic Republic", Abbreviation = "GR", DialingCode = "+30" },
                new CountryDto { Name = "Grenada", FullName = "Grenada", Abbreviation = "GD", DialingCode = "+1-473" },
                new CountryDto { Name = "Guatemala", FullName = "Republic of Guatemala", Abbreviation = "GT", DialingCode = "+502" },
                new CountryDto { Name = "Guinea", FullName = "Republic of Guinea", Abbreviation = "GN", DialingCode = "+224" },
                new CountryDto { Name = "Guinea-Bissau", FullName = "Republic of Guinea-Bissau", Abbreviation = "GW", DialingCode = "+245" },
                new CountryDto { Name = "Guyana", FullName = "Co-operative Republic of Guyana", Abbreviation = "GY", DialingCode = "+592" },
                new CountryDto { Name = "Haiti", FullName = "Haiti", Abbreviation = "HT", DialingCode = "+509" },
                new CountryDto { Name = "Honduras", FullName = "Republic of Honduras", Abbreviation = "HN", DialingCode = "+504" },
                new CountryDto { Name = "Hungary", FullName = "Hungary", Abbreviation = "HU", DialingCode = "+36" },
                new CountryDto { Name = "Iceland", FullName = "Iceland", Abbreviation = "IS", DialingCode = "+354" },
                new CountryDto { Name = "India", FullName = "Republic of India", Abbreviation = "IN", DialingCode = "+91" },
                new CountryDto { Name = "Indonesia", FullName = "Republic of Indonesia", Abbreviation = "ID", DialingCode = "+62" },
                new CountryDto { Name = "Iran", FullName = "Islamic Republic of Iran", Abbreviation = "IR", DialingCode = "+98" },
                new CountryDto { Name = "Iraq", FullName = "Republic of Iraq", Abbreviation = "IQ", DialingCode = "+964" },
                new CountryDto { Name = "Ireland", FullName = "Ireland", Abbreviation = "IE", DialingCode = "+353" },
                new CountryDto { Name = "Israel", FullName = "State of Israel", Abbreviation = "IL", DialingCode = "+972" },
                new CountryDto { Name = "Italy", FullName = "Italian Republic", Abbreviation = "IT", DialingCode = "+39" },
                new CountryDto { Name = "Jamaica", FullName = "Jamaica", Abbreviation = "JM", DialingCode = "+1-876" },
                new CountryDto { Name = "Japan", FullName = "Japan", Abbreviation = "JP", DialingCode = "+81" },
                new CountryDto { Name = "Jordan", FullName = "Hashemite Kingdom of Jordan", Abbreviation = "JO", DialingCode = "+962" },
                new CountryDto { Name = "Kazakhstan", FullName = "Republic of Kazakhstan", Abbreviation = "KZ", DialingCode = "+7" },
                new CountryDto { Name = "Kenya", FullName = "Republic of Kenya", Abbreviation = "KE", DialingCode = "+254" },
                new CountryDto { Name = "Kiribati", FullName = "Republic of Kiribati", Abbreviation = "KI", DialingCode = "+686" },
                new CountryDto { Name = "Korea (North)", FullName = "Democratic People's Republic of Korea", Abbreviation = "KP", DialingCode = "+850" },
                new CountryDto { Name = "Korea (South)", FullName = "Republic of Korea", Abbreviation = "KR", DialingCode = "+82" },
                new CountryDto { Name = "Kuwait", FullName = "State of Kuwait", Abbreviation = "KW", DialingCode = "+965" },
                new CountryDto { Name = "Kyrgyzstan", FullName = "Kyrgyz Republic", Abbreviation = "KG", DialingCode = "+996" },
                new CountryDto { Name = "Laos", FullName = "Lao People's Democratic Republic", Abbreviation = "LA", DialingCode = "+856" },
                new CountryDto { Name = "Latvia", FullName = "Latvia", Abbreviation = "LV", DialingCode = "+371" },
                new CountryDto { Name = "Lebanon", FullName = "Lebanese Republic", Abbreviation = "LB", DialingCode = "+961" },
                new CountryDto { Name = "Lesotho", FullName = "Kingdom of Lesotho", Abbreviation = "LS", DialingCode = "+266" },
                new CountryDto { Name = "Liberia", FullName = "Republic of Liberia", Abbreviation = "LR", DialingCode = "+231" },
                new CountryDto { Name = "Libya", FullName = "State of Libya", Abbreviation = "LY", DialingCode = "+218" },
                new CountryDto { Name = "Liechtenstein", FullName = "Principality of Liechtenstein", Abbreviation = "LI", DialingCode = "+423" },
                new CountryDto { Name = "Lithuania", FullName = "Republic of Lithuania", Abbreviation = "LT", DialingCode = "+370" },
                new CountryDto { Name = "Luxembourg", FullName = "Grand Duchy of Luxembourg", Abbreviation = "LU", DialingCode = "+352" },
                new CountryDto { Name = "Madagascar", FullName = "Republic of Madagascar", Abbreviation = "MG", DialingCode = "+261" },
                new CountryDto { Name = "Malawi", FullName = "Republic of Malawi", Abbreviation = "MW", DialingCode = "+265" },
                new CountryDto { Name = "Malaysia", FullName = "Malaysia", Abbreviation = "MY", DialingCode = "+60" },
                new CountryDto { Name = "Maldives", FullName = "Republic of Maldives", Abbreviation = "MV", DialingCode = "+960" },
                new CountryDto { Name = "Mali", FullName = "Republic of Mali", Abbreviation = "ML", DialingCode = "+223" },
                new CountryDto { Name = "Malta", FullName = "Malta", Abbreviation = "MT", DialingCode = "+356" },
                new CountryDto { Name = "Marshall Islands", FullName = "Republic of the Marshall Islands", Abbreviation = "MH", DialingCode = "+692" },
                new CountryDto { Name = "Mauritania", FullName = "Islamic Republic of Mauritania", Abbreviation = "MR", DialingCode = "+222" },
                new CountryDto { Name = "Mauritius", FullName = "Republic of Mauritius", Abbreviation = "MU", DialingCode = "+230" },
                new CountryDto { Name = "Mexico", FullName = "United Mexican States", Abbreviation = "MX", DialingCode = "+52" },
                new CountryDto { Name = "Micronesia", FullName = "Federated States of Micronesia", Abbreviation = "FM", DialingCode = "+691" },
                new CountryDto { Name = "Moldova", FullName = "Republic of Moldova", Abbreviation = "MD", DialingCode = "+373" },
                new CountryDto { Name = "Monaco", FullName = "Principality of Monaco", Abbreviation = "MC", DialingCode = "+377" },
                new CountryDto { Name = "Mongolia", FullName = "Mongolia", Abbreviation = "MN", DialingCode = "+976" },
                new CountryDto { Name = "Montenegro", FullName = "Montenegro", Abbreviation = "ME", DialingCode = "+382" },
                new CountryDto { Name = "Morocco", FullName = "Kingdom of Morocco", Abbreviation = "MA", DialingCode = "+212" },
                new CountryDto { Name = "Mozambique", FullName = "Republic of Mozambique", Abbreviation = "MZ", DialingCode = "+258" },
                new CountryDto { Name = "Myanmar", FullName = "Republic of the Union of Myanmar", Abbreviation = "MM", DialingCode = "+95" },
                new CountryDto { Name = "Namibia", FullName = "Republic of Namibia", Abbreviation = "NA", DialingCode = "+264" },
                new CountryDto { Name = "Nauru", FullName = "Republic of Nauru", Abbreviation = "NR", DialingCode = "+674" },
                new CountryDto { Name = "Nepal", FullName = "Federal Democratic Republic of Nepal", Abbreviation = "NP", DialingCode = "+977" },
                new CountryDto { Name = "Netherlands", FullName = "Netherlands", Abbreviation = "NL", DialingCode = "+31" },
                new CountryDto { Name = "New Zealand", FullName = "New Zealand", Abbreviation = "NZ", DialingCode = "+64" },
                new CountryDto { Name = "Nicaragua", FullName = "Republic of Nicaragua", Abbreviation = "NI", DialingCode = "+505" },
                new CountryDto { Name = "Niger", FullName = "Niger", Abbreviation = "NE", DialingCode = "+227" },
                new CountryDto { Name = "Nigeria", FullName = "Federal Republic of Nigeria", Abbreviation = "NG", DialingCode = "+234" },
                new CountryDto { Name = "North Macedonia", FullName = "Republic of North Macedonia", Abbreviation = "MK", DialingCode = "+389" },
                new CountryDto { Name = "Norway", FullName = "Kingdom of Norway", Abbreviation = "NO", DialingCode = "+47" },
                new CountryDto { Name = "Oman", FullName = "Sultanate of Oman", Abbreviation = "OM", DialingCode = "+968" },
                new CountryDto { Name = "Pakistan", FullName = "Islamic Republic of Pakistan", Abbreviation = "PK", DialingCode = "+92" },
                new CountryDto { Name = "Palau", FullName = "Republic of Palau", Abbreviation = "PW", DialingCode = "+680" },
                new CountryDto { Name = "Panama", FullName = "Republic of Panama", Abbreviation = "PA", DialingCode = "+507" },
                new CountryDto { Name = "Papua New Guinea", FullName = "Independent State of Papua New Guinea", Abbreviation = "PG", DialingCode = "+675" },
                new CountryDto { Name = "Paraguay", FullName = "Republic of Paraguay", Abbreviation = "PY", DialingCode = "+595" },
                new CountryDto { Name = "Peru", FullName = "Peru", Abbreviation = "PE", DialingCode = "+51" },
                new CountryDto { Name = "Philippines", FullName = "Republic of the Philippines", Abbreviation = "PH", DialingCode = "+63" },
                new CountryDto { Name = "Portugal", FullName = "Portugal", Abbreviation = "PT", DialingCode = "+351" },
                new CountryDto { Name = "Qatar", FullName = "State of Qatar", Abbreviation = "QA", DialingCode = "+974" },
                new CountryDto { Name = "Romania", FullName = "Romania", Abbreviation = "RO", DialingCode = "+40" },
                new CountryDto { Name = "Russia", FullName = "Russian Federation", Abbreviation = "RU", DialingCode = "+7" },
                new CountryDto { Name = "Rwanda", FullName = "Republic of Rwanda", Abbreviation = "RW", DialingCode = "+250" },
                new CountryDto { Name = "Saint Kitts and Nevis", FullName = "Federation of Saint Kitts and Nevis", Abbreviation = "KN", DialingCode = "+1-869" },
                new CountryDto { Name = "Saint Lucia", FullName = "Saint Lucia", Abbreviation = "LC", DialingCode = "+1-758" },
                new CountryDto { Name = "Saint Vincent and the Grenadines", FullName = "Saint Vincent and the Grenadines", Abbreviation = "VC", DialingCode = "+1-784" },
                new CountryDto { Name = "Samoa", FullName = "Samoa", Abbreviation = "WS", DialingCode = "+685" },
                new CountryDto { Name = "San Marino", FullName = "Serenissima Repubblica di San Marino", Abbreviation = "SM", DialingCode = "+378" },
                new CountryDto { Name = "Sao Tome and Principe", FullName = "Democratic Republic of Sao Tome and Principe", Abbreviation = "ST", DialingCode = "+239" },
                new CountryDto { Name = "Saudi Arabia", FullName = "Kingdom of Saudi Arabia", Abbreviation = "SA", DialingCode = "+966" },
                new CountryDto { Name = "Senegal", FullName = "Republic of Senegal", Abbreviation = "SN", DialingCode = "+221" },
                new CountryDto { Name = "Serbia", FullName = "Republic of Serbia", Abbreviation = "RS", DialingCode = "+381" },
                new CountryDto { Name = "Seychelles", FullName = "Republic of Seychelles", Abbreviation = "SC", DialingCode = "+248" },
                new CountryDto { Name = "Sierra Leone", FullName = "Republic of Sierra Leone", Abbreviation = "SL", DialingCode = "+232" },
                new CountryDto { Name = "Singapore", FullName = "Republic of Singapore", Abbreviation = "SG", DialingCode = "+65" },
                new CountryDto { Name = "Slovakia", FullName = "Slovak Republic", Abbreviation = "SK", DialingCode = "+421" },
                new CountryDto { Name = "Slovenia", FullName = "Republic of Slovenia", Abbreviation = "SI", DialingCode = "+386" },
                new CountryDto { Name = "Solomon Islands", FullName = "Solomon Islands", Abbreviation = "SB", DialingCode = "+677" },
                new CountryDto { Name = "Somalia", FullName = "Federal Republic of Somalia", Abbreviation = "SO", DialingCode = "+252" },
                new CountryDto { Name = "South Africa", FullName = "Republic of South Africa", Abbreviation = "ZA", DialingCode = "+27" },
                new CountryDto { Name = "South Sudan", FullName = "Republic of South Sudan", Abbreviation = "SS", DialingCode = "+211" },
                new CountryDto { Name = "Spain", FullName = "Kingdom of Spain", Abbreviation = "ES", DialingCode = "+34" },
                new CountryDto { Name = "Sri Lanka", FullName = "Democratic Socialist Republic of Sri Lanka", Abbreviation = "LK", DialingCode = "+94" },
                new CountryDto { Name = "Sudan", FullName = "Republic of the Sudan", Abbreviation = "SD", DialingCode = "+249" },
                new CountryDto { Name = "Suriname", FullName = "Republic of Suriname", Abbreviation = "SR", DialingCode = "+597" },
                new CountryDto { Name = "Sweden", FullName = "Kingdom of Sweden", Abbreviation = "SE", DialingCode = "+46" },
                new CountryDto { Name = "Switzerland", FullName = "Swiss Confederation", Abbreviation = "CH", DialingCode = "+41" },
                new CountryDto { Name = "Syria", FullName = "Syrian Arab Republic", Abbreviation = "SY", DialingCode = "+963" },
                new CountryDto { Name = "Taiwan", FullName = "Republic of China", Abbreviation = "TW", DialingCode = "+886" },
                new CountryDto { Name = "Tajikistan", FullName = "Republic of Tajikistan", Abbreviation = "TJ", DialingCode = "+992" },
                new CountryDto { Name = "Tanzania", FullName = "United Republic of Tanzania", Abbreviation = "TZ", DialingCode = "+255" },
                new CountryDto { Name = "Thailand", FullName = "Kingdom of Thailand", Abbreviation = "TH", DialingCode = "+66" },
                new CountryDto { Name = "Timor-Leste", FullName = "Democratic Republic of Timor-Leste", Abbreviation = "TL", DialingCode = "+670" },
                new CountryDto { Name = "Togo", FullName = "Togo", Abbreviation = "TG", DialingCode = "+228" },
                new CountryDto { Name = "Tonga", FullName = "Kingdom of Tonga", Abbreviation = "TO", DialingCode = "+676" },
                new CountryDto { Name = "Trinidad and Tobago", FullName = "Republic of Trinidad and Tobago", Abbreviation = "TT", DialingCode = "+1-868" },
                new CountryDto { Name = "Tunisia", FullName = "Tunisian Republic", Abbreviation = "TN", DialingCode = "+216" },
                new CountryDto { Name = "Turkey", FullName = "Republic of Turkey", Abbreviation = "TR", DialingCode = "+90" },
                new CountryDto { Name = "Turkmenistan", FullName = "Turkmenistan", Abbreviation = "TM", DialingCode = "+993" },
                new CountryDto { Name = "Tuvalu", FullName = "Tuvalu", Abbreviation = "TV", DialingCode = "+688" },
                new CountryDto { Name = "Uganda", FullName = "Republic of Uganda", Abbreviation = "UG", DialingCode = "+256" },
                new CountryDto { Name = "Ukraine", FullName = "Ukraine", Abbreviation = "UA", DialingCode = "+380" },
                new CountryDto { Name = "United Arab Emirates", FullName = "United Arab Emirates", Abbreviation = "AE", DialingCode = "+971" },
                new CountryDto { Name = "United Kingdom", FullName = "United Kingdom of Great Britain and Northern Ireland", Abbreviation = "GB", DialingCode = "+44" },
                new CountryDto { Name = "United States", FullName = "United States of America", Abbreviation = "USA", DialingCode = "+1" },
                new CountryDto { Name = "Uruguay", FullName = "Oriental Republic of Uruguay", Abbreviation = "UY", DialingCode = "+598" },
                new CountryDto { Name = "Uzbekistan", FullName = "Republic of Uzbekistan", Abbreviation = "UZ", DialingCode = "+998" },
                new CountryDto { Name = "Vanuatu", FullName = "Republic of Vanuatu", Abbreviation = "VU", DialingCode = "+678" },
                new CountryDto { Name = "Vatican City", FullName = "Vatican City State", Abbreviation = "VA", DialingCode = "+379" },
                new CountryDto { Name = "Venezuela", FullName = "Bolivarian Republic of Venezuela", Abbreviation = "VE", DialingCode = "+58" },
                new CountryDto { Name = "Vietnam", FullName = "Socialist Republic of Vietnam", Abbreviation = "VN", DialingCode = "+84" },
                new CountryDto { Name = "Yemen", FullName = "Republic of Yemen", Abbreviation = "YE", DialingCode = "+967" },
                new CountryDto { Name = "Zambia", FullName = "Republic of Zambia", Abbreviation = "ZM", DialingCode = "+260" },
                new CountryDto { Name = "Zimbabwe", FullName = "Republic of Zimbabwe", Abbreviation = "ZW", DialingCode = "+263" },

            };

            // Add countries to the database
            foreach (var country in countries)
            {
                await countriesService.AddCountryAsync(country);
            }
        }
    }
}
