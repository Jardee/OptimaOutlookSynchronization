# OptimaOutlookSynchro

Program do synchronizowania modułów Serwis oraz CRM z optimy do outlook'a

# Główne funkcje

- [ ] Synchronizacja Moduł Serwis -> Outlook Calendar

  - [x] Automatyczne dodawanie serwisu do kalendarza.
  - [x] Automatyczna aktualizacja w kalendarzu po zmianie w optimie.
  - [x] Automatyczne usuwanie w kalendarzu po usunięciu w optimie.
  - [ ] Powiadamianie o zmianie statusu serwisu osoby dodatkowe SMS/Mail
  
- [ ] Synchronizacja Moduł CRM -> Outlook Calendar

  - [ ] Automatyczne dodawanie Kontaktu do kalendarza.
  - [ ] Automatyczna aktualizacja w kalendarzu po zmianie w optimie.
  - [ ] Automatyczne usuwanie w kalendarzu po usunięciu w optimie.
  

  
  # Opis
 
 Usługa systemu windows która co określony czas (domyślnie 15 minut) sprawdza bazę danych optimy oraz plik z powiązaniami ( Wydarzenie w kalendarzu <-> wpis do bazy danych) w poszukiwaniu nowych wpisów bądź ich zmian i odzwierciedla je w wydarzeniach kalendarza outlook'a
 
 
 #TO-DO
- [x] Customowe (definiowane przez użytkownika) szablony tytułów oraz opisów wydarzenia(osobno serwisu oraz kontaktów) w kalendarzu outlook, możliwość wykonania własnych kwerend SQL.
- [x] Połączenie z bazą danych optimy wyciąganie danych o serwisach
- [ ] Połączenie z bazą danych optimy wyciąganie danych o Kontaktach
- [x] Sposób na śledzenie który wpis do bazy dotyczy jakiego wydarzenia w kaledarzu
  - Zostało to rozwiązane plikiem sqlite który przetrzymuje w jedym wierszu ID wpisu z bazy danych optimy, ID wydarzenia z outlooka, datę ostatniej modyfikacji śledzonego wiersza
- [x] Połączenie programu z Microsoft Graph
- [x] Znaleźć sposób by usługa prosiła o zalogowanie użytkownika microsoft tylko podczas uruchomienia usługi
- [ ] Code refactoring spaghetti
- [ ] Pomyśleć nad testami
- [ ] Wrzucanie logów do bazy danych
- [ ] Wysyłanie SMS/Maila gdy wygaśnie token MGraph i trzeba ponownie się zalogować
- [ ] Może jakiś program graficzny do edycji bazy SQLite configów/odczytywanie logów

  ... Może ulec zmianie gdy wpadnie coś do głowy
