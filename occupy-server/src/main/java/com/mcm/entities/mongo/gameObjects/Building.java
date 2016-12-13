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
        switch (type) {
            case BANK:
                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case MOSQUE:
                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case PARK:
                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
        }
    }

    @Override
    protected double getExperience(double hitPower) {
        return 0;
    }

    @Override
    protected void levelIncreasedBy(double diff) {

    }

    @Override
    protected void healthIncreasedBy(double diff) {

    }

    @Override
    protected void levelDecreasedBy(double diff) {

    }

    @Override
    protected void healthDecreasedBy(double diff) {

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
