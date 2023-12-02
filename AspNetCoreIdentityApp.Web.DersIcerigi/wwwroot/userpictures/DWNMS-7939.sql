
--trf_id = 6000272

INSERT INTO fcbsadm.trf_def (trf_def_id,"name",descr,uom_def_id,mny_tp_id,cmpny_def_id,cdate,cuser,udate,uuser,logcl_tp_id,trf_type,stand_alone_trf_def_id) 
VALUES (6000272,'Price plan',NULL,2,1,1,now(),'DWNMS-7939',NULL,NULL,10,NULL,6000272);

INSERT INTO fcbsadm.chrg_trf_ver (trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
VALUES (6000272,0,'2018-01-01 00:00:00.000','<DecisionTree xmlns=''http://www.i2i.com/common/DecisionTree/schemas''>
  <DecisionPoint>
    <decisionResultIndex>0</decisionResultIndex>
  </DecisionPoint>
<DecisionResult name="Price plan" type="2">
    <ATTRIBUTES NAME="module" VALUE="RateRecurrent">
      <ATTRIBUTES NAME="sequence" VALUE="1">
        <ATTRIBUTES NAME="Value" VALUE="60"/>
        <ATTRIBUTES NAME="Period Type" VALUE="4"/>
        <ATTRIBUTES NAME="Period Length" VALUE="1"/>
        <ATTRIBUTES NAME="Period Start Count" VALUE="1"/>
        <ATTRIBUTES NAME="Period End Count" VALUE="-1"/>
        <ATTRIBUTES NAME="Charge Type" VALUE="1"/>
        <ATTRIBUTES NAME="Prorate Indicator" VALUE="Y"/>  
        <ATTRIBUTES NAME="Prorate On Cancel Indicator" VALUE="Y"/> 
      </ATTRIBUTES>
    </ATTRIBUTES>
  </DecisionResult>
</DecisionTree>',now(),'DWNMS-7939',NULL,NULL);

insert into fcbsadm.chrg_trf_ver(trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
select trf_def_id,1,sdate + interval '1 second',trf_xml,cdate,'DWNMS-7939',udate,uuser 
from fcbsadm.chrg_trf_ver where trf_def_id in (6000272) and ver=0;


insert into fcbsadm.bill_trf_ver(trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
select 6000272,0,sdate,trf_xml,now(),'DWNMS-7939',udate,uuser 
from fcbsadm.bill_trf_ver where trf_def_id in (99999) and ver=0;


insert into fcbsadm.bill_trf_ver(trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
select trf_def_id,1,sdate + interval '1 second',trf_xml,cdate,'DWNMS-7939',udate,uuser 
from fcbsadm.bill_trf_ver where trf_def_id in (6000272) and ver=0;




--trf_id = 6000273

INSERT INTO fcbsadm.trf_def (trf_def_id,"name",descr,uom_def_id,mny_tp_id,cmpny_def_id,cdate,cuser,udate,uuser,logcl_tp_id,trf_type,stand_alone_trf_def_id) 
VALUES (6000273,'Price plan',NULL,2,1,1,now(),'DWNMS-7939',NULL,NULL,10,NULL,6000273);

INSERT INTO fcbsadm.chrg_trf_ver (trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
VALUES (6000273,0,'2018-01-01 00:00:00.000','<DecisionTree xmlns=''http://www.i2i.com/common/DecisionTree/schemas''>
  <DecisionPoint>
    <decisionResultIndex>0</decisionResultIndex>
  </DecisionPoint>
<DecisionResult name="Price plan" type="2">
    <ATTRIBUTES NAME="module" VALUE="RateRecurrent">
      <ATTRIBUTES NAME="sequence" VALUE="1">
        <ATTRIBUTES NAME="Value" VALUE="65"/>
        <ATTRIBUTES NAME="Period Type" VALUE="4"/>
        <ATTRIBUTES NAME="Period Length" VALUE="1"/>
        <ATTRIBUTES NAME="Period Start Count" VALUE="1"/>
        <ATTRIBUTES NAME="Period End Count" VALUE="-1"/>
        <ATTRIBUTES NAME="Charge Type" VALUE="1"/>
        <ATTRIBUTES NAME="Prorate Indicator" VALUE="Y"/>  
        <ATTRIBUTES NAME="Prorate On Cancel Indicator" VALUE="Y"/> 
      </ATTRIBUTES>
    </ATTRIBUTES>
  </DecisionResult>
</DecisionTree>',now(),'DWNMS-7939',NULL,NULL);

insert into fcbsadm.chrg_trf_ver(trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
select trf_def_id,1,sdate + interval '1 second',trf_xml,cdate,'DWNMS-7939',udate,uuser 
from fcbsadm.chrg_trf_ver where trf_def_id in (6000273) and ver=0;


insert into fcbsadm.bill_trf_ver(trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
select 6000273,0,sdate,trf_xml,now(),'DWNMS-7939',udate,uuser 
from fcbsadm.bill_trf_ver where trf_def_id in (99999) and ver=0;


insert into fcbsadm.bill_trf_ver(trf_def_id,ver,sdate,trf_xml,cdate,cuser,udate,uuser) 
select trf_def_id,1,sdate + interval '1 second',trf_xml,cdate,'DWNMS-7939',udate,uuser 
from fcbsadm.bill_trf_ver where trf_def_id in (6000273) and ver=0;





