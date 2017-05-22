package ${entity.javaPackage};
 
 <#-- freemaker 的注释-->
public class ${entity.className}<#if entity.superclass?has_content> extends ${entity.superclass} </#if>
{

    /********** attribute ***********/
<#list entity.properties as property>
    private ${property.javaType} ${property.propertyName};
     
</#list>
    /********** constructors ***********/
<#if entity.constructors>
    public ${entity.className}() {
    
    }
 

</#if>
 
    /********** get/set ***********/
<#list entity.properties as property>
    public ${property.javaType} get${property.propertyName?cap_first}() {
        return ${property.propertyName};
    }
 
    public void set${property.propertyName?cap_first}(${property.javaType} ${property.propertyName}) {
        this.${property.propertyName} = ${property.propertyName};
    }
     
</#list>
}