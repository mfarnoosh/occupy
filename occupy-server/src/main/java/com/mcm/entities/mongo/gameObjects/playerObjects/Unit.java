package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.World;
import com.mcm.entities.mongo.Player;
import com.mcm.enums.UnitPropertyType;
import com.mcm.enums.UnitType;
import com.mcm.util.GameConfig;
import org.apache.log4j.Logger;

import java.util.*;
import java.util.stream.Collectors;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Unit extends BasePlayerObject {
    private UnitType type;
    private String keepingTowerId;
    private boolean isMoving = false;
    private static Logger logger = Logger.getLogger(Unit.class);
    //region Constructors
    public Unit(){}
    public Unit(UnitType type, Player player, Tower keepingTower) {
        setType(type);
        setPlayer(player);
        setKeepingTowerId(keepingTower);
        setLocation(keepingTower.getLocation());

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
    //TODO: Reconsider about reload

    /**
     * reload rate shows how many percent should increase in a period of time
     * ??!!
     *
     * @return reload rate in minute
     */
    public double getReloadRate() {
        return (5 / 9) * (getMaxHitPoint() / getValue());
    }

    /**
     * shows how much time it takes to reload completely.
     *
     * @return time in minutes
     */
    public double getReloadTime() {
        return getReloadNowCost() / GameConfig.getEveryMinuteValue();
    }

    /**
     * player should pay how many gold to make the unit ready just now
     *
     * @return cost of reload now
     */
    public double getReloadNowCost() {
        return 1.5 * ((getMaxHitPoint() - getCurrentHitPoint()) / getMaxHitPoint()) * getValue();
    }

    /**
     * make unit ready to use
     */
    public void reloadNow() {
        //TODO: Farnoosh
    }


    /**
     * show this unit how long takes to arrive to given location
     *
     * @param lat destination lat
     * @param lon destination lon
     * @return less than zero means can not reach there
     */
    public double getArriveTime(double lat, double lon) {
        return World.getArriveTime(this, lat, lon);
    }

    /**
     * see the above overload method doc
     *
     * @param targetTower destination tower
     * @return
     */
    public double getArriveTime(Tower targetTower) {
        if (targetTower.getLocation() == null || targetTower.getLocation().length != 2)
            return -1;
        return getArriveTime(targetTower.getLocation()[0], targetTower.getLocation()[1]);
    }

    /**
     * show this unit is ready to attack or not
     *
     * @return
     */
    public boolean canAttack() {
        return currentHitPoint > 0;
    }

    /**
     * attack to any enemy target in it's range
     * CONSIDER that for unit there is no splash attack and just one target can be attacked.
     */
    public Collection<BasePlayerObject> attack() {
        LinkedHashSet<BasePlayerObject> changedObjects = new LinkedHashSet<>();
        if (canAttack()) {
            Tower targetTower = World.getNearestTowerInUnitRange(this, playerId);
            if (targetTower != null) {
                List<Unit> unitsInTower = World.findUnitByOwnerTower(targetTower).stream().filter(unit -> {return unit.getCurrentHitPoint() > 0;}).collect(Collectors.toList());
                if (unitsInTower.size() > 0) {
                    Optional<Unit> mostPowerfullUnit = unitsInTower.stream().max((o1, o2) -> {
                        return o1.getAttackDamage() > o2.getAttackDamage() ? 1 : -1;
                    });
                    if (mostPowerfullUnit.isPresent()) {
                        attackTo(mostPowerfullUnit.get(), false);
                        changedObjects.add(mostPowerfullUnit.get());
                    }
                } else {
                    double health = targetTower.getCurrentHitPoint() - getFireRate() * (getAttackDamage());
                    logger.info("unit attack -> " + health);
                    targetTower.setCurrentHitPoint(health);
                    changedObjects.add(targetTower);
                }
            }

        }
        return changedObjects;
    }
    public void attackTo(Unit enemy, boolean inTower) {
        //TODO: maybe we should set land damage and air damage for unit
        if (canAttack() && !Objects.equals(enemy.playerId, playerId)) {
            double health = enemy.getCurrentHitPoint() - getFireRate() * (inTower ? getDefenceDamage() : getAttackDamage());
            enemy.setCurrentHitPoint(health);
            logger.info("unit attack to -> " + health);
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
    public boolean isMoving() { return isMoving; }

    public void setMoving(boolean moving) { isMoving = moving; }

    public UnitType getType() { return type; }

    public void setType(UnitType type) { this.type = type; }

    public String getKeepingTowerId() { return keepingTowerId; }

    public void setKeepingTowerId(Tower keepingTower) {
        if (keepingTower != null)
            this.keepingTowerId = keepingTower.getId();
    }
    public void setKeepingTowerId(String keepingTowerId) {
        this.keepingTowerId = keepingTowerId;
    }

    //endregion

    //region config values
    public double getBuildTime() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.BUILD_TIME));
    }

    public double getValue() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.VALUE));
    }

    public double getMaxHitPoint() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.HIT_POINT));
    }

    public double getAttackDamage() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.ATTACK_DAMAGE));
    }

    public double getDefenceDamage() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.DEFENCE_DAMAGE));
    }

    public double getFireRate() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.FIRE_RATE));
    }

    public double getRange() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.RANGE));
    }

    public double getSpeed() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.SPEED));
    }

    public double getUpgradePrice() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.UPGRADE_PRICE));
    }

    public double getUpgradeTime() {
        return Double.parseDouble(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.UPGRADE_TIME));
    }

    public int getHouseSpace(){
        return Integer.parseInt(GameConfig.getUnitProperty(getType(), getLevel(), UnitPropertyType.HOUSE_SPACE));
    }

    public boolean isLandType() {
        return type == UnitType.SOLDIER || type == UnitType.MOTOR || type == UnitType.TANK || type == UnitType.TITAN;
    }



    //endregion
}
