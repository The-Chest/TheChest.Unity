# The Chest Changelog

## v0.2.0-1
### Fixes
  * Project not compiling due merge conflicts

## v0.2.0
### New features
  * **Tooltip**
    * Add **UISlotTooltipHandler** component
    * Add **UITooltip** prefab
### Fixes
  * Item icon layer wrong when using drag 'n drop
### Next Steps
  * v0.3.0
    * [Context Menu](https://github.com/Chingling152/the-world/issues/8)
## What's Changed
* The Chest : Empty Slot & Item layer fixes in https://github.com/Chingling152/the-world/pull/10
* The Chest : Basic Tooltip in https://github.com/Chingling152/the-world/pull/11

## v0.1.0
### New features
  * **Inventory**
    * **Interfaces**
       * **IInventory\<T>**  - Interface with abstract methods for inventories (uses ISlot\<T>)
       * **IInventoryUI\<T>** - Interface with abstract methods for inventory UI (uses ISlotUI\<T>)
    * **Classes**
       * **BaseInventory\<T>** - Default inventory methods for generic classes (extends IInventory\<T>)
       * **Inventory**-  Inventory class for _Item_ class and _Slot_ class (extends BaseInventory\<Item>)
      * **UIInventory** - Inventory UI using _Inventory_ and _Item_
    * **Methods (IInventory\<T>)**
      * **Add** - Adds an item at first empty ISlot\<T>
      * **Add at Index** - Adds an item at a selected ISlot\<T> (can replace if enabled)
      * **Get Item** - Gets the first occurrence of an item and amount from a slot
      * **Get by Index** - Gets an item and amount by slot index 
      * **Get Item Count** - Gets the amount of an item in the inventory
      * **Clear** - Get all items and remove from slot
      * **Move** - Switches items from slots
  * **Slot**
    * **Interfaces**
       * **ISlot\<T>** -  Interface with abstract methods for slots 
       * **ISlotUI\<T>** -  Interface with abstract methods for Slot UI 
    * **Classes**
       * **BaseSlot\<T>** - Default slot methods for generic classes  (extends ISlot\<Item>)
       * **Slot** - Slot class for _Item_ class (extends BaseSlot\<Item>)
       * **UISlot** - Slot UI using  _Item_ 
    * **Methods (ISlot\<T>)**
      * **Add** - Adds an item at the empty ISlot\<T> 
      * **Get Amount** - Gets the amount of the item on the ISlot\<T> 
      * **Get All** - Gets all the items from the ISlot\<T> 
      * **Replace** - Replaces the item of the ISlot\<T> for other one

### Known Issues
* Item icon layer wrong when using drag 'n drop