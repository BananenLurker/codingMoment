import './style.css';
import * as THREE from 'three';

// Setup

const scene = new THREE.Scene();

const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);

const renderer = new THREE.WebGLRenderer({
  canvas: document.querySelector('#bg'),
});

renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(window.innerWidth, window.innerHeight);
camera.position.setZ(30);
camera.position.setX(-3);

renderer.render(scene, camera);

// Icosahedron

const icoGeometry = new THREE.IcosahedronGeometry(10, 3, 16, 100);
const icoMaterial = new THREE.MeshStandardMaterial({ color: 0xff6347, wireframe: true });
const icosahedron = new THREE.Mesh(icoGeometry, icoMaterial);

scene.add(icosahedron);

// Torus Knot

const tkGeometry = new THREE.TorusKnotGeometry(2.5, 1, 100, 4, 45);
const tkMaterial = new THREE.MeshStandardMaterial({ color: 0xffffff, wireframe: false });
const torusKnot = new THREE.Mesh(tkGeometry, tkMaterial);

scene.add(torusKnot);

torusKnot.position.set(-10, 0, 49)

torusKnot.rotation.set(0, 1.55, 0)

// Heart

// const first = 0
// const second = 0
// const heartShape = new THREE.Shape();

// heartShape.moveTo( first + 5, second + 5 );
// heartShape.bezierCurveTo( first + 5, second + 5, first + 4, second, first, second );
// heartShape.bezierCurveTo( first - 6, second, first - 6, second + 7,first - 6, second + 7 );
// heartShape.bezierCurveTo( first - 6, second + 11, first - 3, second + 15.4, first + 5, second + 19 );
// heartShape.bezierCurveTo( first + 12, second + 15.4, first + 16, second + 11, first + 16, second + 7 );
// heartShape.bezierCurveTo( first + 16, second + 7, first + 16, second, first + 10, second );
// heartShape.bezierCurveTo( first + 7, second, first + 5, second + 5, first + 5, second + 5 );

// const heartGeometry = new THREE.ShapeGeometry( heartShape );
// const heartMaterial = new THREE.MeshBasicMaterial( { color: 0x00ff00 } );
// const heart = new THREE.Mesh( heartGeometry, heartMaterial ) ;
// scene.add( heart );

// heart.rotation.set(0, 0, 10)

// Lights

var lights = [];
lights[0] = new THREE.DirectionalLight( 0xffffff, 1 );
lights[0].position.set( -5, 0, 0 );
lights[1] = new THREE.DirectionalLight( 0x11E8BB, 1 );
lights[1].position.set( 0.75, 1, 0.5 );
lights[2] = new THREE.DirectionalLight( 0x8200C9, 1 );
lights[2].position.set( -0.75, -1, 0.5 );
scene.add( lights[0] );
scene.add( lights[1] );
scene.add( lights[2] );

// const pointLight = new THREE.PointLight(0xffffff);
// pointLight.position.set(5, 5, 5);

// const ambientLight = new THREE.AmbientLight(0xffffff);
// scene.add(pointLight, ambientLight);

// Stars

function addStar() {
  const geometry = new THREE.BoxGeometry(2, 2, 2, 4, 4, 4);
  const material = new THREE.MeshStandardMaterial({ color: 0xffffff, wireframe: true });
  const star = new THREE.Mesh(geometry, material);

  const [x, y, z] = Array(3)
    .fill()
    .map(() => THREE.MathUtils.randFloatSpread(150));

  star.position.set(x, y, z);
  star.rotation.set(x, y, z)
  scene.add(star);
}

Array(200).fill().forEach(addStar);

// Background

const spaceTexture = new THREE.TextureLoader().load('/space.jpg');
scene.background = spaceTexture;

// Avatar

const daanTexture = new THREE.TextureLoader().load('/CVcover2.png');

const daan = new THREE.Mesh(new THREE.BoxGeometry(3, 3, 3), new THREE.MeshBasicMaterial({ map: daanTexture }));

scene.add(daan);

daan.position.z = -5;
daan.position.x = 2;

// Moon

const moonTexture = new THREE.TextureLoader().load('/moon.jpg');
const normalTexture = new THREE.TextureLoader().load('/normal.jpg');

const moon = new THREE.Mesh(
  new THREE.SphereGeometry(3, 32, 32),
  new THREE.MeshStandardMaterial({
    map: moonTexture,
    normalMap: normalTexture,
  })
);

scene.add(moon);

moon.position.z = 30;
moon.position.x = -10;

// Scroll Animation

function moveCamera() {
  const t = document.body.getBoundingClientRect().top;

  daan.rotation.y += 0.01;
  daan.rotation.z += 0.01;

  camera.position.z = t * -0.01;
  camera.position.x = t * -0.0002;
  camera.rotation.y = t * -0.0002;
}

document.body.onscroll = moveCamera;
moveCamera();

// Animation Loop

function animate() {
  requestAnimationFrame(animate);

  icosahedron.rotation.x += 0.001;
  icosahedron.rotation.y += 0.0005;
  icosahedron.rotation.z += 0.001;

  moon.rotation.x += 0.005;

  renderer.render(scene, camera);
}

function onWindowResize() {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth, window.innerHeight);
}

window.addEventListener('resize', onWindowResize, false);

animate();

const inputs = document.querySelectorAll('input');
inputs.forEach(function(input) {
  const span = document.createElement('span');
  input.parentNode.insertBefore(span, input.nextSibling);
  input.addEventListener('input', function(event) {
    span.innerHTML = this.value.replace(/\s/g, '&nbsp;');
    this.style.width = span.offsetWidth + 'px';
  });
});

document.getElementById("boven-button-button").onclick = function(){
  window.scrollTo(0, 0);
}