
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

//ส่ง Json ข้อมูลพนักงานทั้งหมด
router.get('/json/emp-all', function (req, res) {
    var sql = "SELECT *  FROM employee";
    runsql(sql, function (data) {
        res.send(data);
    });
});

//แสดงผลหน้าเว็บข้อมูลพนักงานทั้งหมด
router.get('/emp', function (req, res) {
    //res.render('employee_list.pug');
    var sql = "SELECT *  FROM employee";
    runsql(sql, function (record) {
        var data = {
            Employee: record
        };
        res.render('employee_list.pug', data);
    });

});

//ค้นหาพนักงาน Json
router.get('/json/find/:id:title', function (req, res) {
    var id = req.params.id;
    var title = req.params.title;
    var sql = "SELECT * FROM employee WHERE emp_id LIKE '%" + id + "' OR title_id LIKE '%" + title + "'";
    runsql(sql, function (data) {
        res.send(data);
    });
});

//ค้นหาพนักงานหน้าเว็บ
router.post('/search', function (req, res) {
    
    res.redirect('/employee/find/' + req.body.id );
    
});

router.get('/find/:id', function (req, res) {
    var id = req.params.id;
    var sql = "SELECT * FROM employee WHERE title_id LIKE '%" + id + "' ";
    runsql(sql, function (record) {
        var data = {
            Employee: record
        };
        res.render('detail.pug', data);
        console.log(record);
    });

});



/* router.get('/', function (req, res) {
    var id = req.params.id;
    var pass = req.params.password;
    var sql = "SELECT * FROM web_login WHERE username = '" + id + "' AND password = '" + pass + "'";
    var mssql = require('mssql');
    mssql.connect(config, function (err) {
        var cmd = new mssql.Request();
        cmd.query(sql, function (err, data) {
            //res.send(data);
           /* if (String(data) == null) {
                res.send("fuck");
                
            } else {
                res.send(String(data));
               
            }
        });

    });
    
});*/

module.exports = router;


