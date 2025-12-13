# The Chest
Generic Inventory System from Unity.

## Architeture

## Customization
The classes created are generics and you can inherit from it to make your own Inventory with your own features.
* ### Implementing custom classes
   * **IInventory\<T\>  | ISlot\<T\>**
       If you want the max abstraction level you can implement this generic interface.
   * **BaseInventory\<T\> | BaseSlot\<T\>**
      This class has the basic implementarions of its interfaces, so if you can use it to implement other methods or just overrride the base methods.
   * **Inventory | Slot**
       This class has the implementarions of its BaseClasses using the class Item, use this if your base item structure is similar to the Item class.

* ### Implementing custom UI elements
   The recommendation is to create your own classes instead of the interfaces
   * **IInventoryUI\<T,Y\> | ISlotUI\<T,Y\>**
     Work in progress (the current version is not the final recommended).
   * **UIInventory | UISlot**
     The recommendation for now is inherit of these classes (because all the UI components use these classes to reference).

* ### Implementing custom UI components
  * **UISlotClickHandler**
    Click handler for Slot (for now depends of **UISlot**)
  * **UISlotDragHandler**
    Drag \'n Drop handler for Slot (for now depends of **UISlot**)
  * **DropArea**
    Drop area used to trigger drop event (depends of **UISlotDragHandler**)

* ### Implementing custom World GameObjects
   These classes are used for test and is not very recommended to use yet
   * **InventoryManager**
     Singleton Inventory Manager (used just for tests)
   * **WorldItem**
     Representation of and WorldItem (use or inherit if your base item structure is similar to the Item class)
   * **WorldItemClickHandler**
     Uses Click and InventoryManager to adds to an Inventory (used just for tests)

## Example
