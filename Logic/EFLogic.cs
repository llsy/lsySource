        #region �����������
        /// <summary>
        /// ��ȡ�����б�
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
                paginationResult.total = query.LongCount();//��ȡ����

                paginationResult.rows = query.OrderByExtensions(parameter.sort, parameter.order)
                    .Pagination(parameter.page, parameter.rows).ToArray();//��ҳ

                return paginationResult;
            }
            #endregion
        }

        /// <summary>
        /// ��ȡ��������
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
                    throw new ExpectedException("���������ѱ�ɾ��");

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
        /// ���ӳ�������
        /// </summary>
        /// <param name="questionInfo"></param>
        public void AddQuestion(QuestionInfo questionInfo)
        {
            #region
            var nowTime = DateTime.Now;//��ǰʱ��
            using (var ctx = new WKQuanEntities())
            {
                if (!questionInfo.QuestionType.HasValue)
                    throw new ExpectedException("��ѡ���������");
                if (string.IsNullOrEmpty(questionInfo.Question))
                    throw new ExpectedException("����д��������");
                if (string.IsNullOrEmpty(questionInfo.Answer))
                    throw new ExpectedException("����д�ش�����");
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
        /// �༭��������
        /// </summary>
        /// <param name="questionInfo"></param>
        public void EditQuestion(QuestionInfo questionInfo)
        {
            #region
            var nowTime = DateTime.Now;//��ǰʱ��
            using (var ctx = new WKQuanEntities())
            {
                var entity = LogicUtils.NotNull(ctx.Info_CommonProblems.Where(m => m.ID == questionInfo.ID).FirstOrDefault());

                if (entity.IsDelete == true)
                    throw new ExpectedException("���������ѱ�ɾ��");

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
        /// ɾ����������
        /// </summary>
        /// <param name="questionInfo"></param>
        public void DeleteQuestion(QuestionInfo questionInfo)
        {
            #region
            var nowTime = DateTime.Now;//��ǰʱ��
            using (var ctx = new WKQuanEntities())
            {
                var entity = LogicUtils.NotNull(ctx.Info_CommonProblems.Where(m => m.ID == questionInfo.ID).FirstOrDefault());

                if (entity.IsDelete == true)
                    throw new ExpectedException("���������ѱ�ɾ��");

                entity.IsDelete = true;
                entity.LastEditLoginName = questionInfo.LastEditLoginName;
                entity.LastEditCreateTime = nowTime;

                ctx.SaveChanges();
            }
            #endregion
        }
        #endregion