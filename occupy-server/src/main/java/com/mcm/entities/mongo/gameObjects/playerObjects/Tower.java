package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.World;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.enums.TowerPropertyType;
import com.mcm.enums.TowerType;
import com.mcm.network.messages.TowerData;
import com.mcm.util.GameConfig;

import java.util.Iterator;
import java.util.LinkedHashSet;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Tower extends BasePlayerObject {
    private TowerType type;

    //region Constructors
    public Tower() {}

    public Tower(TowerType type, Player player, double[] location) {
        setType(type);
        setLocation(location);
        this.player = player;

        initialize();
    }

    public Tower(int typeValue, Player player, double[] location) {
        this(TowerType.valueOf(typeValue), player, location);
    }
    //endregion

    //region Functions
    /**
     * initialize the some data which should save in db
     */
    private void initialize() {
        setCurrentHitPoint(getMaxHitPoint());
    }

    /**
     * repair rate shows how many hit-point should increase in a period of time
     *
     * @return repair rate in minute
     */
    public double getRepairRate() {
        return (5 / 9) * (getMaxHitPoint() / getValue());
    }

    /**
     * shows how much time it takes to repair completely.
     * @return time in minutes
     */
    public double getRepairTime(){
        return getRepairNowCost() / GameConfig.getEveryMinuteValue();
    }
    /**
     * player should pay how many gold to restore hit-point of tower to max-hit-point
     *
     * @return cost of repair now
     */
    public double getRepairNowCost() {
        return 1.5 * ((getMaxHitPoint() - getCurrentHitPoint()) / getMaxHitPoint()) * getValue();
    }

    /**
     * restore the current hit-point to max-hit-point and decrease the cost from owner
     */
    public void repairNow() {
        //TODO: Farnoosh
    }

    /**
     * shows suggested price of this tower to sell
     * all values which player spent for this tower until this level
     * and minus the value of repairing current tower
     * @return suggested price
     */
    public double getSellPrice() {
        double price = 0;
        for(int i = 1; i <= getLevel(); i++){
            price += Double.parseDouble(GameConfig.getTowerProperty(getType(),i,TowerPropertyType.VALUE));
        }
        price -= getRepairNowCost();

        price *= (2/3);

        return price;
    }

    /**
     * sell this tower to player
     *
     * @param player new owner of tower
     * @param price  agreement price of tower which should took from new owner and gave to old owner of tower
     */
    public void sell(Player player, double price) {}

    /**
     * show this tower is ready to attack or not
     * @return
     */
    public boolean canAttack(){
        if(isUpgrading() || getCurrentHitPoint() == 0)
            return false;
        return true;
    }
    /**
     * if any enemy unit is in range, start to attack to them.
     * determine if splash attack or single attack should apply
     */
    public void attack(){
        if(canAttack()) {
            LinkedHashSet<Unit> others = World.getAllUnitsNearTower(this);
            if (getType() == TowerType.STEALTH) {
                splashAttack(others);
            } else {
                attackTo(others.iterator().next());
            }
        }
    }
    /**
     * splash attack means all units in tower range will be damaged.
     * splash towers start to attack when any enemy be in their range.
     * @param others the enemies which should be attacked
     */
    private void splashAttack(LinkedHashSet<Unit> others) {
        for (Unit other : others) {
            attackTo(other);
        }
    }

    /**
     * attack to one enemy unit in the tower range
     * @param enemy target enemy unit
     */
    private void attackTo(Unit enemy) {
        //TODO: Farnoosh
    }
    //endregion

    //region Override Functions

    /**
     * see parent doc
     */
    @Override
    public void upgrade() {
        //TODO: Farnoosh
    }

    //endregion
    //region Method Accessors
    public TowerType getType() {
        return type;
    }

    public void setType(TowerType type) {
        this.type = type;
    }
    //endregion

    //region config values
    public double getBuildTime() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.BUILD_TIME));
    }

    public double getValue() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.VALUE));
    }

    public double getMaxHitPoint() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.HIT_POINT));
    }

    public double getDamagePerSec() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.DAMAGE));
    }

    public double getFireRate() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.FIRE_RATE));
    }

    public double getRange() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.RANGE));
    }

    public double getMaxCapacity() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.MAX_CAPACITY));
    }

    public double getUpgradePrice() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.UPGRADE_PRICE));
    }

    public double getUpgradeTime() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.UPGRADE_TIME));
    }

    //endregion

//    @Override
//    public String toString() {
//
//        return "Tower{" +
//                "_Id='" + getId() + '\'' +
//                ", _Type='" + String.valueOf(getType().getValue()) + '\'' +
//                ", _PlayerKey='" + player.getId() + '\'' +
//                ", _Lat='" + String.valueOf(getLocation()[0]) + '\'' +
//                ", _Lon='" + String.valueOf(getLocation()[1]) + '\'' +
//                ", _Level='" + String.valueOf(getLevel()) + '\'' +
//                ", _Health='" + String.valueOf(getHealth()) + '\'' +
//                ", _Range=" + String.valueOf(getRange()) +
//                '}';
//    }
}
