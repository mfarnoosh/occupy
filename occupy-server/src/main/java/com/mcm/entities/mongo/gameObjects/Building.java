package com.mcm.entities.mongo.gameObjects;

import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.enums.BuildingType;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Building extends BaseGameObject {
    private BuildingType type;
    public Building(BuildingType type,double[] location){
        setType(type);
        setLocation(location);

        setMechanicValues(type);
    }
    public Building(int typeValue,double[] location) {
        this(BuildingType.valueOf(typeValue),location);
    }
    private void setMechanicValues(BuildingType type) {
        setLevel(1);
        switch (type) {
            case BANK:
                break;
            case MOSQUE:
                break;
            case PARK:
                break;
        }
    }
    public BuildingType getType() {
        return type;
    }

    public void setType(BuildingType type) {
        this.type = type;
    }
    //Constructors

    //End Constructors

    //Abstract Methods

    //End Abstract Methods
}
