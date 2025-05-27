# AMMONOMICON API

Ammonomicon API is a tool that allows you to add **custom bookmark/pages** into the Ammonomicon, with functions and tools that also let you modify pre-existing pages, while making the process of making new pages fairly streamlined and easy.
##
## Classes
- UIBuilder ( *Used to **create new bookmarks/pages**, with BuildBookmark()* )

- UICreationTools ( *Multiple different **helper methods** that let you create custom entries much easier.* )

- CustomActions ( *An array of **subscribable actions** and functions used to add to, or modify pre-existing pages.* )

- StaticData ( *A class housing bits of important data, like custom enums, existing bookmarks, default atlas and **multiple copies of different UI elements** the Ammonomicon uses that you can instantiate and use for your own modifications/ pages.* )

- CustomAmmonomiconPageController ( *The main class the user will use to **create custom Ammonomicon entries**. The user will create a custom that that inherits off of CustomAmmonomiconPageController to use and write their own code.* )

- CustomCallbacks ( **EXPERIMENTAL, BE CAREFUL WHEN USED.** *Adds small dictionaries to make sharing data between mods easier for some level of compatibility.* )

- ExampleSetup ( *Accessible on Github, has an example class of how to setup your custom bookmark/pages, along with an example action from CustomActions* )

##

## Credits

- Bobot (Original creator of AmmonomiconAPI, her code was used/modified/built on top of to create the current structure of AmmonomiconAPI)
- Captain Pretzel (Lots of help with code and debugging.)
- Some Bunny (Nerd.)