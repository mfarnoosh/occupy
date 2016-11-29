package com.mcm.service;

import com.mcm.util.GeoUtil;
import com.mcm.util.SharedPreference;
import org.springframework.data.geo.Point;

import java.util.List;

/**
 * Created by Mehrdad on 16/11/23.
 */
public class Tile {
    private int tileX;
    private int tileY;
    private int zoom;
    private GeoUtil.BoundingBox boundingBox;
    private Point center;

    private byte[] image;

    public Tile(int tileX, int tileY) {
        this(tileX, tileY, MapService.defaultZoomLevel);
    }

    public Tile(int tileX, int tileY, int zoom) {
        setTileX(tileX);
        setTileY(tileY);
        setZoom(zoom);

        setCenter(GeoUtil.getTileCenter(getTileX(), getTileY(), getZoom()));
        setBoundingBox(GeoUtil.tile2boundingBox(getTileX(), getTileY(), getZoom()));
        loadImage();
    }

    public GeoUtil.BoundingBox getBoundingBox() {
        return boundingBox;
    }

    public void setBoundingBox(GeoUtil.BoundingBox boundingBox) {
        this.boundingBox = boundingBox;
    }

    public Tile(double lat, double lon) {
        this(lat, lon, MapService.defaultZoomLevel);
    }

    public Tile(double lat, double lon, int zoom) {
        setZoom(zoom);
        //no need to store requested lat,lon, we need just TileCenter
        List<Integer> tileNumbers = GeoUtil.getTileNumber(lat, lon, zoom);
        setTileX(tileNumbers.get(0));
        setTileY(tileNumbers.get(1));

        setCenter(GeoUtil.getTileCenter(getTileX(), getTileY(), getZoom()));
        setBoundingBox(GeoUtil.tile2boundingBox(getTileX(), getTileY(), getZoom()));
        loadImage();
    }

    private void loadImage() {
        byte[] tileImage = MapService.getTileImage(getTileUrl());
        setImage(tileImage);
    }

    public String getTileUrl() {
        return GeoUtil.getTileNumberUrl(getTileX(), getTileY(), getZoom());
    }

    //accessors
    public int getTileX() {
        return tileX;
    }

    public void setTileX(int tileX) {
        this.tileX = tileX;
    }

    public int getTileY() {
        return tileY;
    }

    public void setTileY(int tileY) {
        this.tileY = tileY;
    }

    public int getZoom() {
        return zoom;
    }

    public void setZoom(int zoom) {
        this.zoom = zoom;
    }

    public Point getCenter() {
        return center;
    }

    public void setCenter(Point point) {
        this.center = point;
    }

    public byte[] getImage() {
        return image;
    }

    public void setImage(byte[] image) {
        this.image = image;
    }
}
