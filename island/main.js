import './style.css'
import * as THREE from 'three';
import {GLTFLoader} from 'three/addons/loaders/GLTFLoader.js';

const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 0.1, 1000);
const renderer = new THREE.WebGLRenderer({
  canvas: document.querySelector('#bg'),
});

renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(window.innerWidth, window.innerHeight);
camera.position.set(15, 12, 15);

var rotSpeed = 0.02;

function checkRotation(){

  var x = camera.position.x,
    z = camera.position.z;

    camera.position.x = x * Math.cos(rotSpeed) + z * Math.sin(rotSpeed);
    camera.position.z = z * Math.cos(rotSpeed) - x * Math.sin(rotSpeed);

  camera.lookAt(scene.position);
}

document.body.onscroll = checkRotation;

renderer.render(scene, camera);

const pointLight = new THREE.PointLight(0xffffff);
pointLight.position.set(20,20,20);

const ambientLight = new THREE.AmbientLight(0xffffff);
scene.add(ambientLight, pointLight);

const lightHelper = new THREE.PointLightHelper(pointLight)
const gridHelper = new THREE.GridHelper(200, 50);
scene.add(lightHelper, gridHelper);

// Floating Island

const loader = new GLTFLoader();

loader.load('/floating island.glb', function (gltf) {
  scene.add(gltf.scene);
});

// Animation loop

function animate(){
  requestAnimationFrame(animate);

  renderer.render(scene, camera);
}

animate();

window.addEventListener("scroll", function() {
  var scrollHeight = document.documentElement.scrollHeight;
  var scrollTop = document.documentElement.scrollTop;
  var clientHeight = document.documentElement.clientHeight;
  
  if (scrollTop + clientHeight >= scrollHeight) {
    window.scrollTo(0, 0);
  }
});