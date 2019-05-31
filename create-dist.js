const del = require('del');
const copyDir = require('copy-dir');
const fs = require('fs');
const {zip} = require('zip-a-folder');

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
    crossPopulatePackages();
    fs.copyFileSync(`${SOURCE}/package.json`, `${OUTPUT}/package.json`);
    await zip(OUTPUT, `${OUTPUT}.zip`);
    
    console.log(`Copied files from ${SOURCE} to ${OUTPUT}`);
}

function copyFiles() {
    copyDir.sync(SOURCE, OUTPUT, {});
    COPY_FILES.forEach((file) => {
        if (!fs.existsSync(file)) return;
        fs.copyFileSync(file, `${OUTPUT}/${file}`);
    });
}

function crossPopulatePackages() {
    copyJsonFields('package.json', `${SOURCE}/package.json`, [
        'name',
        'displayName',
        'description',
        'version',
        'unity',
        'repository',
        'license',
        'bugs',
        'homepage',
        'keywords',
        'author',
    ]);
}

function copyJsonFields(sourcePath, destPath, fields) {
    const source = JSON.parse(fs.readFileSync(sourcePath).toString());
    const dest = JSON.parse(fs.readFileSync(destPath).toString());
    
    fields.forEach((field) => {
       dest[field] = source[field]; 
    });

    fs.writeFileSync(destPath, JSON.stringify(dest, null, 2));
}

init();
