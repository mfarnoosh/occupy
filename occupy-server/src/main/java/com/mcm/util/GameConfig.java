package com.mcm.util;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import com.mcm.enums.*;
import org.w3c.dom.Document;
import org.w3c.dom.NodeList;
import org.w3c.dom.Node;
import org.w3c.dom.Element;
import org.xml.sax.SAXException;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;


/**
 * Created by Mehrdad on 16/12/22.
 */
public class GameConfig {
    private static Document document;
    private static Element currentConfig;
    private static Element currentConfigTowers;
    private static Element currentConfigUnits;
    private static Element currentConfigMap;

    static {
        try {
            document = DocumentBuilderFactory.newInstance()
                    .newDocumentBuilder()
                    .parse(GameConfig.class.getClassLoader().getResourceAsStream("game-config.xml"));
            document.normalize();
            //get active config
            currentConfig = getFirstElementByAttribute(
                    document.getDocumentElement().getChildNodes(),
                    "version",
                    SharedPreference.get("game_config_version"));
            currentConfig.normalize();

            currentConfigTowers = (Element) currentConfig.getElementsByTagName("towers").item(0);
            currentConfigTowers.normalize();
            currentConfigUnits = (Element) currentConfig.getElementsByTagName("units").item(0);
            currentConfigUnits.normalize();
            currentConfigMap = (Element) currentConfig.getElementsByTagName("map").item(0);
            currentConfigMap.normalize();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (SAXException e) {
            e.printStackTrace();
        } catch (ParserConfigurationException e) {
            e.printStackTrace();
        }
    }

    private static Element getFirstElementByAttribute(NodeList nodes, String attributeName, String attributeValue) {
        return getElementsByAttribute(nodes,attributeName,attributeValue).get(0);
    }
    private static List<Element> getElementsByAttribute(NodeList nodes, String attributeName, String attributeValue) {
        List<Element> elements = new ArrayList<>();
        for (int i = 0; i < nodes.getLength(); i++) {
            Node node = nodes.item(i);
            if (node.getNodeType() == Node.ELEMENT_NODE) {
                Element element = (Element) node;
                if (element.getAttribute(attributeName).equalsIgnoreCase(attributeValue)) {
                    elements.add(element);
                }
            }
        }
        return elements;
    }

    public static String getMapProperty(MapConfig mapConfig){
        Element element = (Element) currentConfigMap.getElementsByTagName(mapConfig.toString()).item(0);
        return element.getTextContent();
    }

    public static String getTowerProperty(TowerType type, int level, TowerPropertyType propertyType) {
        Element e= getFirstElementByAttribute(currentConfigTowers.getChildNodes(),"name",type.toString());
        Element e2 = getFirstElementByAttribute(e.getChildNodes(),"level",String.valueOf(level));
        Element e3 = (Element) e2.getElementsByTagName(propertyType.toString()).item(0);
        return e3.getTextContent();
    }
    public static String getUnitProperty(UnitType type, int level, UnitPropertyType propertyType) {
        Element e= getFirstElementByAttribute(currentConfigUnits.getChildNodes(),"name",type.toString());
        Element e2 = getFirstElementByAttribute(e.getChildNodes(),"level",String.valueOf(level));
        Element e3 = (Element) e2.getElementsByTagName(propertyType.toString()).item(0);
        return e3.getTextContent();
    }

    public static int getTowerMaxLevel(TowerType type){
        Element e= getFirstElementByAttribute(currentConfigTowers.getChildNodes(),"name",type.toString());
        return Integer.parseInt(e.getAttribute("max-level"));
    }
    public static int getUnitMaxLevel(UnitType type){
        Element e= getFirstElementByAttribute(currentConfigUnits.getChildNodes(),"name",type.toString());
        return Integer.parseInt(e.getAttribute("max-level"));
    }

    /**
     * this is the basis of game mechanic, which show us every minute in game how many gold worth
     * @return
     */
    public static double getEveryMinuteValue(){
       return 50;
    }
}
