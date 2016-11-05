package com.mcm.util;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

/**
 * Created by Cross on 2/18/2015.
 * This class is only spring context loader.
 * The Spring context can be used by Spring.context, and beans will be loaded by Spring.context.getBean(BEAN NAME).
 * After this initialization @Autowired will work and beans can be autowired.
 */
public class Spring {

    public static ApplicationContext context= new ClassPathXmlApplicationContext("spring-servlet.xml");
}
