<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.4" tiledversion="1.4.2" name="enttilemap" tilewidth="48" tileheight="32" tilecount="13" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="1" type="hatStand">
  <properties>
   <property name="hatId" type="int" value="0"/>
   <property name="name" value="hatStand"/>
  </properties>
  <image width="16" height="32" source="../../gfx/entities/hatStand.png"/>
 </tile>
 <tile id="2" type="hero">
  <properties>
   <property name="name" value="hero"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/hero.png"/>
 </tile>
 <tile id="3" type="berry">
  <properties>
   <property name="name" value="berry"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/berry.png"/>
 </tile>
 <tile id="4" type="hatch">
  <properties>
   <property name="name" value="hatch"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/hatch.png"/>
 </tile>
 <tile id="5" type="spring">
  <properties>
   <property name="name" value="spring"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/spring.png"/>
 </tile>
 <tile id="6">
  <properties>
   <property name="capacity" type="int" value="2"/>
   <property name="name" value="hatStapler"/>
  </properties>
  <image width="48" height="32" source="../../gfx/entities/static/hatStabler.png"/>
 </tile>
 <tile id="7">
  <properties>
   <property name="name" value="sign"/>
   <property name="styleId" type="int" value="0"/>
   <property name="text" value=""/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/sign.png"/>
 </tile>
 <tile id="8">
  <properties>
   <property name="name" value="scroll"/>
  </properties>
  <image width="24" height="24" source="../../gfx/entities/static/scroll.png"/>
 </tile>
 <tile id="9">
  <properties>
   <property name="downwardTiles" type="int" value="1"/>
   <property name="name" value="ladder"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/ladder.png"/>
 </tile>
 <tile id="10">
  <properties>
   <property name="exitId" type="int" value="0"/>
   <property name="exitMap" value=""/>
   <property name="glassId" type="int" value="0"/>
   <property name="name" value="marmalade"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/marmalade.png"/>
 </tile>
 <tile id="11">
  <properties>
   <property name="name" value="spike"/>
  </properties>
  <image width="16" height="16" source="../../gfx/entities/static/spikes.png"/>
 </tile>
 <tile id="12">
  <properties>
   <property name="checkpointId" type="int" value="0"/>
   <property name="name" value="checkpoint"/>
  </properties>
  <image width="32" height="32" source="../../gfx/entities/static/checkpoint.png"/>
 </tile>
 <tile id="13">
  <properties>
   <property name="name" value="sneezePlant"/>
  </properties>
  <image width="32" height="32" source="../../gfx/entities/static/sneezePlant.png"/>
 </tile>
</tileset>
