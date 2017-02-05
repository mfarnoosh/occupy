package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.World;
import com.mcm.entities.mongo.Player;
import com.mcm.enums.TowerPropertyType;
import com.mcm.enums.TowerType;
import com.mcm.util.GameConfig;

import java.util.*;
import java.util.stream.Collectors;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Tower extends BasePlayerObject {
    private TowerType type;
    private int occupiedHouseSpace = 0;
    //region Constructors
    public Tower() {}

    public Tower(TowerType type, Player player, double[] location) {
        setType(type);
        setLocation(location);
        setPlayer(player);

        initialize();
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
    public Collection<BasePlayerObject> attack(){
        LinkedHashSet<BasePlayerObject> changedObjects = new LinkedHashSet<>();
        if(canAttack()) {
            LinkedHashSet<Unit> others = World.getAllUnitsNearTower(this, playerId);
            List<Unit> unitsInTower = World.findUnitByOwnerTower(this).stream().filter(unit -> {return unit.getCurrentHitPoint() > 0;}).collect(Collectors.toList());
            if (getType() == TowerType.STEALTH) {
                for (Unit u: others) {
                    attackTo(u);
                    changedObjects.add(u);
                }
            } else {
                if (others.size() > 0) {
                    Unit first = others.iterator().next();
                    attackTo(first);
                    changedObjects.add(first);
                }
            }

            if (unitsInTower.size() > 0) {
                if (others.size() > 0) {
                    Optional<Unit> mostPowerfullUnit = unitsInTower.stream().max((o1, o2) -> {
                        return o1.getAttackDamage() > o2.getAttackDamage() ? 1 : -1;
                    });
                    Optional<Unit> aliveUnit = others.stream().filter(unit -> {return unit.currentHitPoint > 0;}).findFirst();
                    if (mostPowerfullUnit.isPresent() && aliveUnit.isPresent()) {
                        mostPowerfullUnit.get().attackTo(aliveUnit.get(), true);
                        changedObjects.add(aliveUnit.get());
                    }
                }

            }


        }
        return changedObjects;
    }


    /**
     * attack to one enemy unit in the tower range
     * @param enemy target enemy unit
     */
    private void attackTo(Unit enemy) {
        if (canAttack() && !Objects.equals(enemy.playerId, playerId)) {
            enemy.setCurrentHitPoint(enemy.getCurrentHitPoint() - getFireRate() * (enemy.isLandType() ? getLandDamage() : getAirDamage()));
        }
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
    public int getOccupiedHouseSpace() { return occupiedHouseSpace; }

    public void setOccupiedHouseSpace(int occupiedHouseSpace) { this.occupiedHouseSpace = occupiedHouseSpace; }
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

    public double getAirDamage() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.AIR_DAMAGE));
    }

    public double getLandDamage() {
        return Double.parseDouble(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.LAND_DAMAGE));
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

    public int getMaxHouseSpace(){
        return Integer.parseInt(GameConfig.getTowerProperty(getType(), getLevel(), TowerPropertyType.MAX_HOUSE_SPACE));
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
