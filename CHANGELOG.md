# v0.5.0

## What's changed
* Project now have the following structure:
  * package.json - Unity Package information
  * README.md - Project readme
  * CHANGELOG.md - Project changelog
  * Runtime/ - Project's source code
  * Tests/ - Project's test code (empty for now)
  * Editor/ - Project's editor code (empty for now)
  * Samples/ - Project's Examples

## What's next
* Add unit tests from `TheChest.Core` and `TheChest.Inventories`
* Build and tests pipelines
* Package creation pipeline

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.3.0...v0.4.0

# v0.4.0

## What's Changed
* Removes everything from outside the folder `Assets`
* The project is now using [TheChest.Core v0.8.0](https://github.com/The-Chest/TheChest.Core/releases/tag/v0.8.0) and [TheChest.Inventories v0.8.1](https://github.com/The-Chest/TheChest.Inventories/releases/tag/v0.8.1)

## What's next
* This version and earlier are being discontinued, and the project will have a full reset
  * Plan new architecture
  * Plan new goals and versions

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.3.0...v0.4.0

# v0.3.0

## What's Added
* The project is now using [The Chest Inventory v0.7.0](https://github.com/The-Chest/TheChest.Inventories/releases/v0.7.0)
## Known Issues
* The project needs a great refactor, every version before this will be considered Obsolete

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.2.2...v0.3.0

# v0.2.2

## What's Added
* `IStackContainer<T>`, `IStackInventory<T>` and `IContainer<T>` interfaces
* `IInventorySlot<T>`, `IStackSlot<T>` and `IInventoryStackSlot<T>` interfaces
## What's Changed
* The whole structure of `Stack` and `Container` code
## What's Removed
* `Weapon`, `Consumable`, `Types`

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.2.1...v0.2.2
# v0.2.1
## What's added
* `IContainer<T>`, `IInteractiveContainer<T>`  and `IInventory<T>` interfaces
## What's changed 
* The whole project folder architecture
## What's removed
* Inventory property naming
* Deprecated Interfaces and Base Classes of Inventory
* Linq methods usage

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.2.0-1...v0.2.1

# v0.2.0-1
## Fixes
  * Project not compiling due merge conflicts

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.2.0...v0.2.0-1

# v0.2.0
## New features
  * **Tooltip**
    * Add **UISlotTooltipHandler** component
    * Add **UITooltip** prefab
## Fixes
  * Item icon layer wrong when using drag 'n drop
## Next Steps
  * v0.3.0
    * [Context Menu](https://github.com/Chingling152/the-world/issues/8)
## What's Changed
* The Chest : Empty Slot & Item layer fixes in https://github.com/Chingling152/the-world/pull/10
* The Chest : Basic Tooltip in https://github.com/Chingling152/the-world/pull/11

**Full Changelog**: https://github.com/The-Chest/TheChest.Unity/compare/v0.1.0...v0.2.0

# v0.1.0
## New features
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

## Known Issues
* Item icon layer wrong when using drag 'n drop