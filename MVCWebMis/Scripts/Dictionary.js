/*字典 Dictionary类*/
function Dictionary() {
    this.add = add;
    this.datastore = new Array();
    this.find = find;
    this.remove = remove;
    this.showAll = showAll;
    this.count = count;
    this.clear = clear;
}

function add(key, value) {
    this.datastore[key] = value;
}

function find(key) {
    return this.datastore[key];
}

function remove(key) {
    delete this.datastore[key];
}

function showAll() {
    var str = "";
    for (var key in this.datastore) {
        str += key + " -> " + this.datastore[key] + ";  "
    }
    console.log(str);
}

function count() {
    /*var ss = Object.keys(this.datastore).length;
    console.log("ssss   "+ss);
    return Object.keys(this.datastore).length;*/
    /**/
    var n = 0;
    for (var key in Object.keys(this.datastore)) {
        ++n;
    }
    console.log(n);
    return n;
}

function clear() {
    for (var key in this.datastore) {
        delete this.datastore[key];
    }
}