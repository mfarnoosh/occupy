package com.mcm.entities.mongo;

import com.mcm.entities.MultilingualValue;
import org.springframework.data.mongodb.core.index.GeoSpatialIndexed;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class Player extends BaseDocument {
    private MultilingualValue name;
    private MultilingualValue lastName;
    @GeoSpatialIndexed
    private double[] lastLocation;
    private String email;
    private String username;
    private String password;
    private String phoneNo;

    public MultilingualValue getName() {
        return name;
    }

    public void setName(MultilingualValue name) {
        this.name = name;
    }

    public MultilingualValue getLastName() {
        return lastName;
    }

    public void setLastName(MultilingualValue lastName) {
        this.lastName = lastName;
    }

    public double[] getLastLocation() {
        return lastLocation;
    }

    public void setLastLocation(double[] lastLocation) {
        this.lastLocation = lastLocation;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPhoneNo() {
        return phoneNo;
    }

    public void setPhoneNo(String phoneNo) {
        this.phoneNo = phoneNo;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
