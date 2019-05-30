const del = require('del');
const copyDir = require('copy-dir');
const fs = require('fs');

const OUTPUT = 'dist';
const SOURCE = 'Assets/FluidBehaviorTree';
const COPY_FILES = [
    'README.md',
    'CHANGELOG.md',
    'LICENSE.md',
];

async function init () {
    await del([OUTPUT]);
    
    copyFiles();
    updateVersionNumber();
}

function copyFiles() {
    copyDir.sync(SOURCE, OUTPUT, {});
    COPY_FILES.forEach((file) => {
        if (!fs.existsSync(file)) return;
        fs.copyFileSync(file, `${OUTPUT}/${file}`);
    });
}

function updateVersionNumber() {
    const npmPackage = JSON.parse(fs.readFileSync('package.json').toString());
    const unityPackage = JSON.parse(fs.readFileSync(`${SOURCE}/package.json`).toString());
    unityPackage.version = npmPackage.version;
    fs.writeFileSync(`${OUTPUT}/package.json`, JSON.stringify(unityPackage, null, 2));
}

init();
