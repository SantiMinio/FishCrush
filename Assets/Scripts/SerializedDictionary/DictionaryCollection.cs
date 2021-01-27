using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FishSpawnProbability : SerializableDictionary<Fish, int> { }

[Serializable]
public class NodesList : SerializableDictionary<NodePosition, List<Node>, NodeStorage> { }

[Serializable]
public class NodeStorage : SerializableDictionary.Storage<List<Node>> { }

