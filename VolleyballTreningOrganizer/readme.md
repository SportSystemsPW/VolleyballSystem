# Volleyball Trening Organizer
##  Cel projektu
Celem projektu jest stworzenie aplikacji mobilnej dla trenerów pozwalającej na organizowanie treningów oraz zarządzania opłatami za treningi. Trenujący będą powiadamiani o treningach za pośrednictwem wiadomości SMS, odpowiadając na które mogą zgłosić swoją obecność lub nie obecność. Za każdy trening zostanie naliczona opłata ustalona przez trenera.
##  Technologia
Aplikacja zostanie wykonana w nowoczesnym framework'u do aplikacji mobilnych .NET MAUI przy użyciu wzorca projektowego Model-View-ViewModel.
##  Opis aplikacji
Aplikacja będzie składała się z 4 zakładek.
###  Szablon wiadomości
W tej zakładce użytkownik będzie mógł wyświetlać, dodawać, edytować i usuwać szablony wiadomości SMS, które będą wysyłane do trenujących. Szablony będą zawierały zmienne takie jak data treningu oraz cena, w miejsce których automatycznie będą podstawiane wartości ustalone przez trenera.
###  Grupy trenujących
W tej zakładce użytkownik będzie zarządzał grupami trenujących, aby ułatwić zapraszanie ich na treningi. Użytkownik będzie mógł dodać osobę do grupy z kontaktów na telefonie lub ręcznie.
###  Treningi
W tej zakładce użytkownik będzie mógł tworzyć treningi. Podczas tworzenia treningu trener wybiera datę, koszt treningu, grupę uczestników oraz szablon wiadomości, która zostanie wysłana do zaproszonych osób. Trener będzie mógł też przeglądać wszystkie inne swoje treningi, oraz wyświetlać szczegóły poszczególnych treningów. W szczegółach znajdują się wszystkie dane które trener wprowadził przy tworzeniu oraz lista obecności, tworzona na podstawie odpowiedzi SMS trenujących. Listę obecności trener będzie mógł edytować ręcznie.
###  Opłaty
W tej zakładce trener będzie mógł przeglądać należności wszystkich swoich podopiecznych oraz zaznaczać otrzymane opłaty.
