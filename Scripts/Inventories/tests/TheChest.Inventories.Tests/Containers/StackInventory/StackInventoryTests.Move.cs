namespace TheChest.Inventories.Tests.Containers
{
    //TODO: find a way to create a complex inventory for this test 
    // with multiple items in each slot
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.Move(origin, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.Move(0, target), Throws.TypeOf<ArgumentOutOfRangeException>());
        }


        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void Move_EmptyOrigin_TargetWithItems_MovesItem()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var originIndex = 0;
            var targetIndex = 10;

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);
            var targetItems = inventory[targetIndex].Content;

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[originIndex].Content, Is.EqualTo(targetItems));
                Assert.That(inventory[targetIndex].IsEmpty, Is.True);
            });
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void Move_OriginWithItems_EmptyTarget_MovesItem()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var originIndex = 0;
            var targetIndex = 10;

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);
            var originItems = inventory[originIndex].Content;

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[originIndex].IsEmpty, Is.True);
                Assert.That(inventory[targetIndex].Content, Is.EqualTo(originItems));
            });
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void Move_OriginAndTargetWithSameItems_MovesItemToOrigin()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var originIndex = 0;
            var targetIndex = 10;

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);
            var originItems = inventory[originIndex].Content;
            var targetItems = inventory[originIndex].Content;

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[originIndex].Content, Is.EqualTo(targetItems));
                Assert.That(inventory[targetIndex].Content, Is.EqualTo(originItems));
            });
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void Move_OriginAndTargetWithDifferentItems_MovesItemToOrigin()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var originIndex = 0;
            var targetIndex = 10;

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);
            var originItems = inventory[originIndex].Content;
            var targetItems = inventory[originIndex].Content;

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[originIndex].Content, Is.EqualTo(targetItems));
                Assert.That(inventory[targetIndex].Content, Is.EqualTo(originItems));
            });
        }

        [Test]
        [Ignore("This test is not working well due Inventory creation")]
        public void Move_OriginAndTargetWithDifferentItems_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var originIndex = 0;
            var targetIndex = 10;

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);
            var originItems = inventory[originIndex].Content;
            var targetItems = inventory[originIndex].Content;
            inventory.OnMove += (o, e) =>
            {
                Assert.That(o, Is.EqualTo(inventory));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(originItems));
                    Assert.That(firstEvent.FromIndex, Is.EqualTo(originIndex));
                    Assert.That(firstEvent.ToIndex, Is.EqualTo(targetIndex));
                });
                Assert.Multiple(() =>
                {
                    var secondEvent = e.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Is.EqualTo(targetItems));
                    Assert.That(secondEvent.FromIndex, Is.EqualTo(targetIndex));
                    Assert.That(secondEvent.ToIndex, Is.EqualTo(originIndex));
                });
            };
            inventory.Move(originIndex, targetIndex);
        }
    }
}
