# Volleyball Trening Organizer
##  Cel projektu
Celem projektu jest stworzenie aplikacji mobilnej dla trenerów pozwalaj¹cej na organizowanie treningów oraz zarz¹dzania op³atami za treningi. Trenuj¹cy bêd¹ powiadamiani o treningach za poœrednictwem wiadomoœci SMS, odpowiadaj¹c na które mog¹ zg³osiæ swoj¹ obecnoœæ lub nie obecnoœæ. Za ka¿dy trening zostanie naliczona op³ata ustalona przez trenera.
##  Technologia
Aplikacja zostanie wykonana w nowoczesnym framework'u do aplikacji mobilnych .NET MAUI przy u¿yciu wzorca projektowego Model-View-ViewModel.
##  Opis aplikacji
Aplikacja bêdzie sk³ada³a siê z 4 zak³adek.
###  Szablon wiadomoœci
W tej zak³adce u¿ytkownik bêdzie móg³ wyœwietlaæ, dodawaæ, edytowaæ i usuwaæ szablony wiadomoœci SMS, które bêd¹ wysy³ane do trenuj¹cych. Szablony bêd¹ zawiera³y zmienne takie jak data treningu oraz cena, w miejsce których automatycznie bêd¹ podstawiane wartoœci ustalone przez trenera.
###  Grupy trenuj¹cych
W tej zak³adce u¿ytkownik bêdzie zarz¹dza³ grupami trenuj¹cych, aby u³atwiæ zapraszanie ich na treningi. U¿ytkownik bêdzie móg³ dodaæ osobê do grupy z kontaktów na telefonie lub rêcznie.
###  Treningi
W tej zak³adce u¿ytkownik bêdzie móg³ tworzyæ treningi. Podczas tworzenia treningu trener wybiera datê, koszt treningu, grupê uczestników oraz szablon wiadomoœci, która zostanie wys³ana do zaproszonych osób. Trener bêdzie móg³ te¿ przegl¹daæ wszystkie inne swoje treningi, oraz wyœwietlaæ szczegó³y poszczególnych treningów. W szczegó³ach znajduj¹ siê wszystkie dane które trener wprowadzi³ przy tworzeniu oraz lista obecnoœci, tworzona na podstawie odpowiedzi SMS trenuj¹cych. Listê obecnoœci trener bêdzie móg³ edytowaæ rêcznie.
###  Op³aty
W tej zak³adce trener bêdzie móg³ przegl¹daæ nale¿noœci wszystkich swoich podopiecznych oraz zaznaczaæ otrzymane op³aty.
