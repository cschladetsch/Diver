﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyro.Unity3d.Scene.Parser
{
    class IdToType
    {
        // see https://docs.unity3d.com/Manual/ClassIDReference.html

        /*
1	GameObject
2	Component
3	LevelGameManager
4	Transform
5	TimeManager
6	GlobalGameManager
8	Behaviour
9	GameManager
11	AudioManager
12	ParticleAnimator
13	InputManager
15	EllipsoidParticleEmitter
17	Pipeline
18	EditorExtension
19	Physics2DSettings
20	Camera
21	Material
23	MeshRenderer
25	Renderer
26	ParticleRenderer
27	Texture
28	Texture2D
29	SceneSettings
30	GraphicsSettings
33	MeshFilter
41	OcclusionPortal
43	Mesh
45	Skybox
47	QualitySettings
48	Shader
49	TextAsset
50	Rigidbody2D
51	Physics2DManager
53	Collider2D
54	Rigidbody
55	PhysicsManager
56	Collider
57	Joint
58	CircleCollider2D
59	HingeJoint
60	PolygonCollider2D
61	BoxCollider2D
62	PhysicsMaterial2D
64	MeshCollider
65	BoxCollider
66	SpriteCollider2D
68	EdgeCollider2D
72	ComputeShader
74	AnimationClip
75	ConstantForce
76	WorldParticleCollider
78	TagManager
81	AudioListener
82	AudioSource
83	AudioClip
84	RenderTexture
87	MeshParticleEmitter
88	ParticleEmitter
89	Cubemap
90	Avatar
91	AnimatorController
92	GUILayer
93	RuntimeAnimatorController
94	ScriptMapper
95	Animator
96	TrailRenderer
98	DelayedCallManager
102	TextMesh
104	RenderSettings
108	Light
109	CGProgram
110	BaseAnimationTrack
111	Animation
114	MonoBehaviour
115	MonoScript
116	MonoManager
117	Texture3D
118	NewAnimationTrack
119	Projector
120	LineRenderer
121	Flare
122	Halo
123	LensFlare
124	FlareLayer
125	HaloLayer
126	NavMeshAreas
127	HaloManager
128	Font
129	PlayerSettings
130	NamedObject
131	GUITexture
132	GUIText
133	GUIElement
134	PhysicMaterial
135	SphereCollider
136	CapsuleCollider
137	SkinnedMeshRenderer
138	FixedJoint
140	RaycastCollider
141	BuildSettings
142	AssetBundle
143	CharacterController
144	CharacterJoint
145	SpringJoint
146	WheelCollider
147	ResourceManager
148	NetworkView
149	NetworkManager
150	PreloadData
152	MovieTexture
153	ConfigurableJoint
154	TerrainCollider
155	MasterServerInterface
156	TerrainData
157	LightmapSettings
158	WebCamTexture
159	EditorSettings
160	InteractiveCloth
161	ClothRenderer
162	EditorUserSettings
163	SkinnedCloth
164	AudioReverbFilter
165	AudioHighPassFilter
166	AudioChorusFilter
167	AudioReverbZone
168	AudioEchoFilter
169	AudioLowPassFilter
170	AudioDistortionFilter
171	SparseTexture
180	AudioBehaviour
181	AudioFilter
182	WindZone
183	Cloth
184	SubstanceArchive
185	ProceduralMaterial
186	ProceduralTexture
191	OffMeshLink
192	OcclusionArea
193	Tree
194	NavMeshObsolete
195	NavMeshAgent
196	NavMeshSettings
197	LightProbesLegacy
198	ParticleSystem
199	ParticleSystemRenderer
200	ShaderVariantCollection
205	LODGroup
206	BlendTree
207	Motion
208	NavMeshObstacle
210	TerrainInstance
212	SpriteRenderer
213	Sprite
214	CachedSpriteAtlas
215	ReflectionProbe
216	ReflectionProbes
218	Terrain
220	LightProbeGroup
221	AnimatorOverrideController
222	CanvasRenderer
223	Canvas
224	RectTransform
225	CanvasGroup
226	BillboardAsset
227	BillboardRenderer
228	SpeedTreeWindAsset
229	AnchoredJoint2D
230	Joint2D
231	SpringJoint2D
232	DistanceJoint2D
233	HingeJoint2D
234	SliderJoint2D
235	WheelJoint2D
238	NavMeshData
240	AudioMixer
241	AudioMixerController
243	AudioMixerGroupController
244	AudioMixerEffectController
245	AudioMixerSnapshotController
246	PhysicsUpdateBehaviour2D
247	ConstantForce2D
248	Effector2D
249	AreaEffector2D
250	PointEffector2D
251	PlatformEffector2D
252	SurfaceEffector2D
258	LightProbes
271	SampleClip
272	AudioMixerSnapshot
273	AudioMixerGroup
290	AssetBundleManifest
1001	Prefab
1002	EditorExtensionImpl
1003	AssetImporter
1004	AssetDatabase
1005	Mesh3DSImporter
1006	TextureImporter
1007	ShaderImporter
1008	ComputeShaderImporter
1011	AvatarMask
1020	AudioImporter
1026	HierarchyState
1027	GUIDSerializer
1028	AssetMetaData
1029	DefaultAsset
1030	DefaultImporter
1031	TextScriptImporter
1032	SceneAsset
1034	NativeFormatImporter
1035	MonoImporter
1037	AssetServerCache
1038	LibraryAssetImporter
1040	ModelImporter
1041	FBXImporter
1042	TrueTypeFontImporter
1044	MovieImporter
1045	EditorBuildSettings
1046	DDSImporter
1048	InspectorExpandedState
1049	AnnotationManager
1050	PluginImporter
1051	EditorUserBuildSettings
1052	PVRImporter
1053	ASTCImporter
1054	KTXImporter
1101	AnimatorStateTransition
1102	AnimatorState
1105	HumanTemplate
1107	AnimatorStateMachine
1108	PreviewAssetType
1109	AnimatorTransition
1110	SpeedTreeImporter
1111	AnimatorTransitionBase
1112	SubstanceImporter
1113	LightmapParameters
1120	LightmapSnapshot

Classes Ordered Alphabetically
Class	ID
ASTCImporter	1053
AnchoredJoint2D	229
Animation	111
AnimationClip	74
Animator	95
AnimatorController	91
AnimatorOverrideController	221
AnimatorState	1102
AnimatorStateMachine	1107
AnimatorStateTransition	1101
AnimatorTransitionBase	1111
AnimatorTransition	1109
AnnotationManager	1049
AreaEffector2D	249
AssetBundle	142
AssetBundleManifest	290
AssetDatabase	1004
AssetImporter	1003
AssetMetaData	1028
AssetServerCache	1037
AudioBehaviour	180
AudioChorusFilter	166
AudioClip	83
AudioDistortionFilter	170
AudioEchoFilter	168
AudioFilter	181
AudioHighPassFilter	165
AudioImporter	1020
AudioListener	81
AudioLowPassFilter	169
AudioManager	11
AudioMixer	240
AudioMixerController	241
AudioMixerEffectController	244
AudioMixerGroup	273
AudioMixerGroupController	243
AudioMixerSnapshot	272
AudioMixerSnapshotController	245
AudioReverbFilter	164
AudioReverbZone	167
AudioSource	82
Avatar	90
AvatarMask	1011
BaseAnimationTrack	110
Behaviour	8
BillboardAsset	226
BillboardRenderer	227
BlendTree	206
BoxCollider	65
BoxCollider2D	61
BuildSettings	141
CachedSpriteAtlas	214
Camera	20
Canvas	223
CanvasGroup	225
CanvasRenderer	222
CapsuleCollider	136
CGProgram	109
CharacterController	143
CharacterJoint	144
CircleCollider2D	58
Cloth	183
ClothRenderer	161
Collider	56
Collider2D	53
Component	2
ComputeShader	72
ComputeShaderImporter	1008
ConfigurableJoint	153
ConstantForce	75
ConstantForce2D	247
Cubemap	89
DDSImporter	1046
DefaultAsset	1029
DefaultImporter	1030
DelayedCallManager	98
DistanceJoint2D	232
EdgeCollider2D	68
EditorBuildSettings	1045
EditorExtension	18
EditorExtensionImpl	1002
EditorSettings	159
EditorUserBuildSettings	1051
EditorUserSettings	162
Effector2D	248
EllipsoidParticleEmitter	15
FBXImporter	1041
FixedJoint	138
Flare	121
FlareLayer	124
Font	128
GameManager	9
GameObject	1
GlobalGameManager	6
GraphicsSettings	30
GUIDSerializer	1027
GUIElement	133
GUILayer	92
GUIText	132
GUITexture	131
Halo	122
HaloLayer	125
HaloManager	127
HierarchyState	1026
HingeJoint	59
HingeJoint2D	233
HumanTemplate	1105
InputManager	13
InspectorExpandedState	1048
InteractiveCloth	160
Joint	57
Joint2D	230
KTXImporter	1054
LensFlare	123
LevelGameManager	3
LibraryAssetImporter	1038
Light	108
LightmapParameters	1113
LightmapSettings	157
LightmapSnapshot	1120
LightProbeGroup	220
LightProbes	258
LightProbesLegacy	197
LineRenderer	120
LODGroup	205
MasterServerInterface	155
Material	21
Mesh	43
Mesh3DSImporter	1005
MeshCollider	64
MeshFilter	33
MeshParticleEmitter	87
MeshRenderer	23
ModelImporter	1040
MonoBehaviour	114
MonoImporter	1035
MonoManager	116
MonoScript	115
Motion	207
MovieImporter	1044
MovieTexture	152
NamedObject	130
NativeFormatImporter	1034
NavMeshAgent	195
NavMeshAreas	126
NavMeshData	238
NavMeshObsolete	194
NavMeshObstacle	208
NavMeshSettings	196
NetworkManager	149
NetworkView	148
NewAnimationTrack	118
OcclusionArea	192
OcclusionPortal	41
OffMeshLink	191
ParticleAnimator	12
ParticleEmitter	88
ParticleRenderer	26
ParticleSystem	198
ParticleSystemRenderer	199
PhysicMaterial	134
Physics2DManager	51
Physics2DSettings	19
PhysicsManager	55
PhysicsMaterial2D	62
PhysicsUpdateBehaviour2D	246
Pipeline	17
PlatformEffector2D	251
PlayerSettings	129
PluginImporter	1050
PointEffector2D	250
PolygonCollider2D	60
Prefab	1001
PreloadData	150
PreviewAssetType	1108
ProceduralMaterial	185
ProceduralTexture	186
Projector	119
PVRImporter	1052
QualitySettings	47
RaycastCollider	140
RectTransform	224
ReflectionProbe	215
ReflectionProbes	216
Renderer	25
RenderSettings	104
RenderTexture	84
ResourceManager	147
Rigidbody	54
Rigidbody2D	50
RuntimeAnimatorController	93
SampleClip	271
SceneAsset	1032
SceneSettings	29
ScriptMapper	94
Shader	48
ShaderImporter	1007
ShaderVariantCollection	200
SkinnedCloth	163
SkinnedMeshRenderer	137
Skybox	45
SliderJoint2D	234
SparseTexture	171
SphereCollider	135
SpringJoint	145
SpringJoint2D	231
Sprite	213
SpriteCollider2D	66
SpriteRenderer	212
SpeedTreeImporter	1110
SpeedTreeWindAsset	228
SubstanceArchive	184
SubstanceImporter	1112
SurfaceEffector2D	252
TagManager	78
Terrain	218
TerrainCollider	154
TerrainData	156
TerrainInstance	210
TextAsset	49
TextMesh	102
TextScriptImporter	1031
Texture	27
Texture2D	28
Texture3D	117
TextureImporter	1006
TimeManager	5
TrailRenderer	96
Transform	4
Tree	193
TrueTypeFontImporter	1042
WebCamTexture	158
WheelCollider	146
WheelJoint2D	235
WindZone	182
WorldParticleCollider	76
*/
    }
}