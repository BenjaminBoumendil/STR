﻿using UnityEngine;
using UnityEngine.Networking;

public class HexCell : AGameObject {

    private HexCoordinates coordinates;
    public HexCoordinates Coordinates {
        get
        {
            return coordinates;
        }
        set
        {
            coordinates = value;
        }
    }
}