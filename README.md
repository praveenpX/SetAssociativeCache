# SetAssociativeCache


Implementation Summary:

Fun challenge to implement SetAssociativeCache using .NET and C#, I’ve created the following objects
•	CacheBlock.cs – This data structure is based on the Entry data structure
•	CacheBlockSet.cs – This data structure is based on the Sachet data structure
•	SetAssociativeCache.cs – Contains the actual implementation of the n-way cache 

In addition to the above, the following two built-in data structures have been leveraged
•	Generic Dictionary object
•	LinkedList object to generate, build the LRU(Least Recently Used) Algorithm and aid the above data structures during the CRUD operations.
•	Additionally, lock object has been used to handle thread-safety. Ideally would have liked to use the built-in Synchronized Collections (ex: ConcurrentDictionary) but not sure if this is allowed in this exercise.

Additional thoughts:
1.	How would you allow the caller to specify their own algorithm for determing which entry to remove?
Comment: I would create an abstract factory and a couple implementations (ex: One for LRU, One for MRU etc..) and have the end-user specify which one to inject as part of the SetAssociativeCache Object initialization.

2.	What improvements could be made to the locking to improve operations
Comment: Leverage ConcurrentDictionary if possible as it’s available in .NET 4.0

3.	How hard it would be to implement standard .NET collection interfaces?
Comment: It’s a time-consuming exercise, as we would have to accommodate for multi-threading scenarios and synchronization for reads, writes. Latest versions of the .NET framework 4.0 provides us some of the data structures that we can leverage to simplify the implementation
