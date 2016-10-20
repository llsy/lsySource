public IPaginationResult<MMemberFinanceIncomeInfo> GetList(GetListParameter parameter)
        {
            #region
            var paginationResult = new PaginationResult<MMemberFinanceIncomeInfo>();

            var sqlWhere = new StringBuilder(" ");
            var sqlWhere2 = new StringBuilder(" ");
            if (!string.IsNullOrEmpty(parameter.LoginName))
                sqlWhere.AppendFormat(" and LoginName='{0}' ", parameter.LoginName);
            if (!string.IsNullOrEmpty(parameter.SerialNumber))
            {
                sqlWhere.AppendFormat(" and SerialNumber='{0}' ", parameter.SerialNumber);
            }
            if (!string.IsNullOrEmpty(parameter.Remark))
            {
                sqlWhere.AppendFormat(" and Remark LIKE '%{0}%' ", parameter.Remark);
            }
            if (parameter.IncomeStatus.HasValue)
            {
                sqlWhere.AppendFormat(" and IncomeStatus='{0}' ", (int)parameter.IncomeStatus.Value);
            }
            if (parameter.IncomeType.HasValue)
            {
                sqlWhere.AppendFormat(" and IncomeType='{0}' ", (int)parameter.IncomeType.Value);
            }
            if (parameter.CreateUser.HasValue)
            {
                sqlWhere2.AppendFormat(" and CreateUser='{0}' ", parameter.CreateUser.ToString());
            }
            if (parameter.DateType.HasValue)
            {
                DateTime? start;
                DateTime? end;
                if (DateExtensions.TryGetDateRange(parameter.DateType, parameter.BeginDate, parameter.EndDate, out start, out end))
                {
                    sqlWhere.AppendFormat(" and CreateTime>='{0}' and CreateTime<'{1}'", start, end);
                }
            }
            //��������
            var sqlSBListCount = new StringBuilder(@"select count(*) from 
(
SELECT * From(
	SELECT *,CASE WHEN isnull(B.bloginname,'')!='' THEN '����Ա' 
			ELSE  (CASE A.CreateLoginName 
				WHEN 'WindowsService' THEN 'ϵͳ' 
				WHEN 'ϵͳ' THEN 'ϵͳ' 
                WHEN 'system' THEN 'ϵͳ' 
				ELSE '�̼�' 
				END)
			END AS CreateUser from
	(SELECT m1.CreateTime,
	m1.ID,
	m1.IncomeAccount,
	m1.PayAccount,
	m1.IncomeMoney,
	m1.IncomeStatus,
	m1.IncomeType,
	m1.SerialNumber,
	m1.LoginName,
	m1.Remark,
	m1.LastEditTime,
	m1.LastEditLoginName LastEditUser,
	m1.CreateLoginName
	FROM M_Member_FinanceIncome m1 WHERE 1=1 {0}
	)A
	outer apply
	(
		select b1.LoginName bloginname
		from Base_Admin b1 
		where A.CreateLoginName=b1.LoginName
	)B  
)C 
WHERE 1=1 {1}
)T");
            //���ֶ�
            var sqlSBList = new StringBuilder(@"
SELECT * From(
	SELECT *,CASE WHEN isnull(B.bloginname,'')!='' THEN '����Ա' 
			ELSE  (CASE A.CreateLoginName 
				WHEN 'WindowsService' THEN 'ϵͳ' 
				WHEN 'ϵͳ' THEN 'ϵͳ' 
                WHEN 'system' THEN 'ϵͳ' 
				ELSE '�̼�' 
				END)
			END AS CreateUser from
	(SELECT m1.CreateTime,
	m1.ID,
	m1.IncomeAccount,
	m1.PayAccount,
	m1.IncomeMoney,
	m1.IncomeStatus,
	m1.IncomeType,
	m1.SerialNumber,
	m1.LoginName,
	m1.Remark,
	m1.LastEditTime,
	m1.LastEditLoginName LastEditUser,
	m1.CreateLoginName
	FROM M_Member_FinanceIncome m1 WHERE 1=1 {0}
	)A
	outer apply
	(
		select b1.LoginName bloginname
		from Base_Admin b1 
		where A.CreateLoginName=b1.LoginName
	)B  
)C 
WHERE 1=1 {1}");

            var sqlListCount = string.Format(sqlSBListCount.ToString(), sqlWhere,sqlWhere2);
            var sqlList = string.Format(sqlSBList.ToString(), sqlWhere, sqlWhere2);
            using (var ctx = new WKQuanEntities())
            {
                paginationResult.total = ctx.ExecuteStoreQuery<int>(sqlListCount).FirstOrDefault();//��ȡ����

                if (parameter.PaginationFilterType == PaginationFilterType.Serarch)
                    paginationResult.rows = ctx.ExecuteStoreQuery<MMemberFinanceIncomeInfo>
                        (sqlList.OrderByPagination(parameter.page, parameter.rows, parameter.sort, parameter.order)).ToArray();//��ҳ
                else
                    paginationResult.rows = ctx.ExecuteStoreQuery<MMemberFinanceIncomeInfo>(sqlList, parameter.sort, parameter.order).ToArray(); ;
            }
            return paginationResult;
}