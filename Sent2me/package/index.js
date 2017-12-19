
/**Employee Management**/

var path = require('path');
var express = require('express');
var router = express.Router();
var bodyParser = require('body-parser');

router.use(bodyParser.json());
router.use(bodyParser.urlencoded({
    extended: true
}));

/*router.use(function (req, res) {
    res.status(404).send({ url: req.originalUrl + ' not found' })
});*/

/*SQL CONNECTION CONFIGULATION*/

var config = {
    user: "sa",
    password: "1234",
    database: "sent2me",
    server: "localhost"
};

/*QUERY MSSQL*/
function runsql(sql, callback) {
    var mssql = require("mssql");

    mssql.connect(config, function (err) {
        if (err) {
            console.log(err);
            return;
        }
        else {
            var cmd = new mssql.Request();
            cmd.query(sql, function (err, data) {
                if (err) {
                    console.log(err);
                    return;
                }
                else {
                    callback(data);
                }
            });
        }

    });

}



/*ROUTE*/

//ส่ง Json ข้อมูลพัสดุทั้งหมด
router.get('/json/dev-all', function (req, res) {
    var sql = "SELECT * FROM Delivery INNER JOIN Package ON Package.package_id = Delivery.package_id INNER JOIN Package_detail ON Package.package_id = Package_detail.package_id INNER JOIN Package_type ON Package_detail.package_type_id = Package_type.package_type_id";
    runsql(sql, function (data) {
        res.send(data);
    });
});

router.get('/json/dev/find/:id', function (req, res) {
    var sql = "SELECT * FROM Delivery WHERE Delivery.dev_id LIKE '%" + req.params.id + "'";
    runsql(sql, function (data) {
        if (data.length > 0) {
            res.send(true);
            console.log("DELIVERY ID:"+data+" LOAD!!");
        } else {
            res.send(false);
        }
    });     
});

router.get('/json/dev/:id', function (req, res) {
    var sql = "SELECT Delivery.dev_id,Delivery_detail.branch_id,time,Delivery_detail.emp_id,Branch.branch_name,Delivery_detail.status,branch_add,latlong,Delivery_desciption.des_text,Package_type.package_type_name FROM Delivery INNER JOIN Delivery_detail ON Delivery.dev_id = Delivery_detail.dev_id INNER JOIN Delivery_desciption ON Delivery_detail.dev_des_id = Delivery_desciption.dev_des_id INNER JOIN Package ON Package.package_id = Delivery_detail.package_id INNER JOIN Package_detail ON Package.package_id = Package_detail.package_id INNER JOIN Package_type ON Package_detail.package_type_id = Package_type.package_type_id INNER JOIN Branch ON Branch.branch_id = Delivery_detail.branch_id WHERE Delivery_detail.dev_id LIKE '%"+req.params.id+"' ORDER BY Delivery_detail.time DESC";
    runsql(sql, function (data) {
        var rec = {}
        rec.all = data
        res.send(rec);
    });
});

/*//ค้นหาพนักงานหน้าเว็บ
router.post('/search', function (req, res) {
    console.log(req.body.id);
    res.redirect('/employee/find:' + req.body.id);

});*/

module.exports = router;


