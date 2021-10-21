using Godot;
using System;
using System.Collections.Generic;
using Game.ScEvents;

namespace Game
{
    public partial class GameController
    {
        private Dictionary<String, PackedScene> propList = new Dictionary<string, PackedScene>();
        private bool isMapAlreadyLoaded = false;
        public void LoadTiledMap(String mapName, int checkpointId = -1)
        {
            if (isMapAlreadyLoaded)
            {
                Node oldmap = GetNode("GameMap");
                oldmap.QueueFree();
                oldmap.Name = "OldMap";
                Node nExistingBackground = GetNode("Background");
                if (nExistingBackground != null)
                {
                    nExistingBackground.QueueFree();
                }
            }

            localSaveGame = LocalSaveGame.Load(mapName, generalSaveGame.slotId);
            localTotalBerries = 0;
            berries = 0;
            localTotalScrolls = 0;
            scrolls = 0;
            generalSaveGame.currentMap = mapName;
            if (!isMapAlreadyLoaded)
            {
                checkpointId = generalSaveGame.saveSpotId;
            }

            Node map = ResourceLoader.Load<PackedScene>("res://maps/" + mapName + ".tmx").Instance();
            map.Name = "GameMap";
            AddChild(map);
            if (map.HasMeta("background"))
            {
                String backgroundname = (string)map.GetMeta("background");
                CanvasLayer nBackground = (CanvasLayer)ResourceLoader.Load<PackedScene>("res://props/backgrounds/" + backgroundname + ".tscn").Instance();
                AddChild(nBackground);
            }
            map.GetNode<Node2D>("back").ZIndex = -2;

            Godot.Collections.Array metaEntities = map.GetNode("ents").GetChildren();
            foreach (Node2D metaEntity in metaEntities)
            {
                String name = (String)metaEntity.GetMeta("name");
                metaEntity.QueueFree();
                PackedScene loadedProp;
                if ((loadedProp = GetPropLoadOnDemand(name)) == null)
                {
                    continue;
                }
                Node2D nn = (Node2D)loadedProp.Instance();
                nn.Position = metaEntity.Position + new Vector2(8, -24);
                metaEntity.GetParent().AddChild(nn);
                switch (name)
                {
                    case "hatch": nn.Position += new Vector2(0, -8); break;
                    case "scroll":
                        if (localSaveGame.scrollsCollected.Contains(localTotalScrolls))
                        {
                            scrolls++;
                            nn.QueueFree();
                        }
                        else
                        {
                            Scroll nScroll = (Scroll)nn;
                            nScroll.SetId(localTotalScrolls);
                            nScroll.Position += new Vector2(4, -4);
                        }
                        localTotalScrolls++;
                        break;
                    case "hatStand":
                        nn.Position += new Vector2(0, -16);
                        HatRespawnPoint hatResPoint = (HatRespawnPoint)nn;
                        hatResPoint.HatIndex = (int)metaEntity.GetMeta("hatId");
                        break;
                    case "hero":
                        if (!generalSaveGame.newGame)
                        {
                            nn.Free();
                            continue;
                        }
                        nHero = (Hero)nn;
                        nHero.AddHat(0);
                        generalSaveGame.newGame = false;
                        break;
                    case "berry":
                        if (localSaveGame.berriesCollected.Contains(localTotalBerries))
                        {
                            berries++;
                            nn.Free();
                        }
                        else
                        {
                            Berry nBerry = (Berry)nn;
                            nBerry.SetId(localTotalBerries);
                        }
                        localTotalBerries++;
                        break;
                    case "sign":
                        Sign nSign = (Sign)nn;
                        nSign.SetText((String)metaEntity.GetMeta("text"));
                        nSign.SetStyle((int)metaEntity.GetMeta("styleId"));
                        break;
                    case "ladder":
                        Ladder nLadder = (Ladder)nn;
                        nLadder.SetLength((int)metaEntity.GetMeta("downwardTiles"));
                        break;
                    case "hatStapler":
                        nn.Position += new Vector2(16, -8);
                        HatStapler hatStapler = (HatStapler)nn;
                        hatStapler.SetHatCapacity((int)metaEntity.GetMeta("capacity"));
                        break;
                    case "marmalade":
                        Marmalade nMarmalade = (Marmalade)nn;
                        int glassId = (int)metaEntity.GetMeta("glassId");
                        int exitId = (int)metaEntity.GetMeta("exitId");
                        string exitMap = (string)metaEntity.GetMeta("exitMap");
                        nMarmalade.Setup(glassId, exitId, exitMap);
                        if (glassId == checkpointId && generalSaveGame.isAtMarmalade)
                        {
                            nHero = (Hero)GetPropLoadOnDemand("hero").Instance();
                            nHero.Position = nMarmalade.Position + new Vector2(0, -2);
                            metaEntity.GetParent().AddChild(nHero);
                            nMarmalade.waitUntilLeft = true;
                        }
                        break;
                    case "checkpoint":
                        Checkpoint checkpoint = (Checkpoint)nn;
                        checkpoint.Position += new Vector2(8, -8);
                        int ckId = (int)metaEntity.GetMeta("checkpointId");
                        checkpoint.SetId(ckId);
                        if (ckId == checkpointId && !generalSaveGame.isAtMarmalade)
                        {
                            nHero = (Hero)GetPropLoadOnDemand("hero").Instance();
                            nHero.Position = checkpoint.Position;
                            metaEntity.GetParent().AddChild(nHero);
                        }
                        break;
                    case "sneezePlant":
                        List<int> berryIds = new List<int>();
                        for (int i = 0; i < 6; i++)
                        {
                            if (!localSaveGame.berriesCollected.Contains(localTotalBerries + i))
                            {
                                berryIds.Add(localTotalBerries + i);
                            }
                            else
                            {
                                berries++;
                            }
                        }
                        SneezePlant nSneezePlant = (SneezePlant)nn;
                        nSneezePlant.setBerries(berryIds);
                        nSneezePlant.Position += new Vector2(8, -8);
                        localTotalBerries += 6;
                        break;
                }
            }
            Node2D markerGrp;
            if ((markerGrp = map.GetNode<Node2D>("marker")) != null)
            {
                LoadTiledMapMarkers(markerGrp.GetChildren());
            }
            isMapAlreadyLoaded = true;
            guiController.UpdateBerryCounter();
        }

        private void LoadTiledMapMarkers(Godot.Collections.Array metaNodes)
        {
            foreach (Node2D metaNode in metaNodes)
            {
                metaNode.QueueFree();
                switch ((string)metaNode.GetMeta("name"))
                {
                    case "FallJoke":
                        if (DidEventPass("FallJk"))
                        {
                            continue;
                        }
                        FallJoke fallJoke = (FallJoke)ResourceLoader.Load<PackedScene>("res://props/unique/FallJoke.tscn").Instance();
                        fallJoke.Position = metaNode.Position;
                        Node nColShape = metaNode.GetChild(0);
                        metaNode.RemoveChild(nColShape);
                        fallJoke.AddChild(nColShape);
                        nColShape.Owner = fallJoke;
                        metaNode.GetParent().AddChild(fallJoke);
                        break;
                    case "PetalEffectZone":
                        Particles2D pedalParticleSystem = (Particles2D)ResourceLoader.Load<PackedScene>
                        ("res://props/unique/PetalParticleSystem.tscn").Instance();
                        CollisionShape2D collisionShape = (CollisionShape2D)metaNode.GetChild(0);
                        RectangleShape2D rect = (RectangleShape2D)collisionShape.Shape;
                        Vector3 particleSystemExtends = new Vector3(rect.Extents.x, rect.Extents.y, 1);
                        pedalParticleSystem.Position = metaNode.Position + new Vector2(rect.Extents.x / 2, rect.Extents.y/2);
                        ((ParticlesMaterial)pedalParticleSystem.ProcessMaterial).EmissionBoxExtents = particleSystemExtends;
                        metaNode.GetParent().AddChild(pedalParticleSystem);
                        break;
                }
            }
        }

        private PackedScene GetPropLoadOnDemand(string key)
        {
            if (!propList.ContainsKey(key))
            {
                string uppercaseKey = char.ToUpper(key[0]) + key.Substring(1);
                propList.Add(key, (PackedScene)ResourceLoader.Load("res://props/levelEnts/" + uppercaseKey + ".tscn"));
            }

            return propList[key];
        }
    }
}