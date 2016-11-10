        #region 常见问题管理
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IPaginationResult<QuestionInfo> GetQuestionList(QuestionParameter parameter)
        {
            #region
            Expression<Func<Info_CommonProblem, bool>> wherelambda =
                (m) => m.IsDelete == false;
            if (parameter.QuestionType.HasValue)
                wherelambda = wherelambda.And(m => m.ProblemType == (int)parameter.QuestionType);

            using (var ctx = new WKQuanEntities())
            {
                var paginationResult = new PaginationResult<QuestionInfo>();
                var query = ctx.Info_CommonProblems.Where(wherelambda).Select(m => new QuestionInfo()
                {
                    ID = m.ID,
                    QuestionType = (QuestionType?)m.ProblemType,
                    Question = m.Problem,
                    Answer = m.Answer,
                    LastEditTime = m.LastEditCreateTime
                });
                paginationResult.total = query.LongCount();//获取总数

                paginationResult.rows = query.OrderByExtensions(parameter.sort, parameter.order)
                    .Pagination(parameter.page, parameter.rows).ToArray();//分页

                return paginationResult;
            }
            #endregion
        }

        /// <summary>
        /// 获取单条问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuestionInfo GetQuestion(int id)
        {
            #region
            using (var ctx = new WKQuanEntities())
            {
                var entity = LogicUtils.NotNull(ctx.Info_CommonProblems.Where(n => n.ID == id).FirstOrDefault());
                if (entity.IsDelete == true)
                    throw new ExpectedException("该条数据已被删除");

                return new QuestionInfo()
                {
                    ID = entity.ID,
                    QuestionType = (QuestionType)entity.ProblemType,
                    Question = entity.Problem,
                    Answer = entity.Answer,
                };
            }
            #endregion
        }

        /// <summary>
        /// 增加常见问题
        /// </summary>
        /// <param name="questionInfo"></param>
        public void AddQuestion(QuestionInfo questionInfo)
        {
            #region
            var nowTime = DateTime.Now;//当前时间
            using (var ctx = new WKQuanEntities())
            {
                if (!questionInfo.QuestionType.HasValue)
                    throw new ExpectedException("请选择问题分类");
                if (string.IsNullOrEmpty(questionInfo.Question))
                    throw new ExpectedException("请填写问题内容");
                if (string.IsNullOrEmpty(questionInfo.Answer))
                    throw new ExpectedException("请填写回答内容");
                ctx.AddToInfo_CommonProblems(new Info_CommonProblem()
                {
                    ProblemType = (int)questionInfo.QuestionType,
                    Problem = questionInfo.Question,
                    IsDelete = false,
                    Answer = questionInfo.Answer,
                    CreateLoginName = questionInfo.CreateLoginName,
                    CreateTime = nowTime,
                    LastEditLoginName = questionInfo.CreateLoginName,
                    LastEditCreateTime = nowTime,
                });
                ctx.SaveChanges();
            }
            #endregion
        }

        /// <summary>
        /// 编辑常见问题
        /// </summary>
        /// <param name="questionInfo"></param>
        public void EditQuestion(QuestionInfo questionInfo)
        {
            #region
            var nowTime = DateTime.Now;//当前时间
            using (var ctx = new WKQuanEntities())
            {
                var entity = LogicUtils.NotNull(ctx.Info_CommonProblems.Where(m => m.ID == questionInfo.ID).FirstOrDefault());

                if (entity.IsDelete == true)
                    throw new ExpectedException("该条数据已被删除");

                if (questionInfo.QuestionType != null) entity.ProblemType = (int)questionInfo.QuestionType;
                entity.Problem = questionInfo.Question;
                entity.Answer = questionInfo.Answer;
                entity.LastEditLoginName = questionInfo.LastEditLoginName;
                entity.LastEditCreateTime = nowTime;

                ctx.SaveChanges();
            }
            #endregion
        }

        /// <summary>
        /// 删除常见问题
        /// </summary>
        /// <param name="questionInfo"></param>
        public void DeleteQuestion(QuestionInfo questionInfo)
        {
            #region
            var nowTime = DateTime.Now;//当前时间
            using (var ctx = new WKQuanEntities())
            {
                var entity = LogicUtils.NotNull(ctx.Info_CommonProblems.Where(m => m.ID == questionInfo.ID).FirstOrDefault());

                if (entity.IsDelete == true)
                    throw new ExpectedException("该条数据已被删除");

                entity.IsDelete = true;
                entity.LastEditLoginName = questionInfo.LastEditLoginName;
                entity.LastEditCreateTime = nowTime;

                ctx.SaveChanges();
            }
            #endregion
        }
        #endregion