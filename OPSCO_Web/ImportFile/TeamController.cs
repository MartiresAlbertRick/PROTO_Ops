namespace OPSCO_Web.Controllers
{
    using Microsoft.CSharp.RuntimeBinder;
    using OPSCO_Web.Models;
    using PagedList;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;

    public class TeamController : Controller
    {
        private OSCContext db = new OSCContext();

        public ActionResult Create()
        {
            try
            {
                string str = base.get_Session()["role"].ToString();
                string str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__3.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__3.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanAdd", typeof(TeamController), argumentInfo));
                }
                <>o__3.<>p__0.Target((CallSite) <>o__3.<>p__0, base.get_ViewBag(), this.db.appFacade.CanAdd(str3, "Team"));
                if (<>o__3.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__3.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__3.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__3.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__3.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__3.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanAdd", typeof(TeamController), argumentInfo));
                }
                if (<>o__3.<>p__3.Target((CallSite) <>o__3.<>p__3, <>o__3.<>p__2.Target((CallSite) <>o__3.<>p__2, <>o__3.<>p__1.Target((CallSite) <>o__3.<>p__1, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            if (<>o__3.<>p__4 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__3.<>p__4 = CallSite<Func<CallSite, object, SelectList, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Departments", typeof(TeamController), argumentInfo));
            }
            <>o__3.<>p__4.Target((CallSite) <>o__3.<>p__4, base.get_ViewBag(), new SelectList(this.db.Departments, "DepartmentId", "DepartmentName"));
            return base.View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TeamId,TeamName,DepartmentId,IsActive,BIUserGroup,AIQUserGroup")] OSC_Team oSC_Team)
        {
            try
            {
                string str = base.get_Session()["role"].ToString();
                string str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__4.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__4.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanAdd", typeof(TeamController), argumentInfo));
                }
                <>o__4.<>p__0.Target((CallSite) <>o__4.<>p__0, base.get_ViewBag(), this.db.appFacade.CanAdd(str3, "Team"));
                if (<>o__4.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__4.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__4.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__4.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__4.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__4.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanAdd", typeof(TeamController), argumentInfo));
                }
                if (<>o__4.<>p__3.Target((CallSite) <>o__4.<>p__3, <>o__4.<>p__2.Target((CallSite) <>o__4.<>p__2, <>o__4.<>p__1.Target((CallSite) <>o__4.<>p__1, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            oSC_Team.IsActive = true;
            if (base.get_ModelState().get_IsValid())
            {
                this.db.Teams.Add(oSC_Team);
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            return base.View(oSC_Team);
        }

        public ActionResult Delete(long? id)
        {
            string str;
            string str2;
            try
            {
                str = base.get_Session()["role"].ToString();
                str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__7.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__7.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanDelete", typeof(TeamController), argumentInfo));
                }
                <>o__7.<>p__0.Target((CallSite) <>o__7.<>p__0, base.get_ViewBag(), this.db.appFacade.CanDelete(str3, "Team"));
                if (<>o__7.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__7.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__7.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__7.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__7.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__7.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanDelete", typeof(TeamController), argumentInfo));
                }
                if (<>o__7.<>p__3.Target((CallSite) <>o__7.<>p__3, <>o__7.<>p__2.Target((CallSite) <>o__7.<>p__2, <>o__7.<>p__1.Target((CallSite) <>o__7.<>p__1, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(400);
            }
            object[] objArray1 = new object[] { id };
            OSC_Team team = this.db.Teams.Find(objArray1);
            if (team == null)
            {
                return base.HttpNotFound();
            }
            if (!this.db.IsManaged(new long?(team.TeamId), str2, str))
            {
                return base.HttpNotFound();
            }
            object[] objArray2 = new object[] { team.DepartmentId };
            team.Department = this.db.Departments.Find(objArray2);
            return base.View(team);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                string str = base.get_Session()["role"].ToString();
                string str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__8.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__8.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanDelete", typeof(TeamController), argumentInfo));
                }
                <>o__8.<>p__0.Target((CallSite) <>o__8.<>p__0, base.get_ViewBag(), this.db.appFacade.CanDelete(str3, "Team"));
                if (<>o__8.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__8.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__8.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__8.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__8.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__8.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanDelete", typeof(TeamController), argumentInfo));
                }
                if (<>o__8.<>p__3.Target((CallSite) <>o__8.<>p__3, <>o__8.<>p__2.Target((CallSite) <>o__8.<>p__2, <>o__8.<>p__1.Target((CallSite) <>o__8.<>p__1, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            object[] objArray1 = new object[] { id };
            OSC_Team team = this.db.Teams.Find(objArray1);
            if (team == null)
            {
                return base.HttpNotFound();
            }
            team.IsActive = false;
            if (base.get_ModelState().get_IsValid())
            {
                this.db.Entry<OSC_Team>(team).set_State(0x10);
                this.db.SaveChanges();
            }
            return base.RedirectToAction("Index");
        }

        public ActionResult Details(long? id)
        {
            string str;
            string str2;
            try
            {
                str = base.get_Session()["role"].ToString();
                str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__2.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__2.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanView", typeof(TeamController), argumentInfo));
                }
                <>o__2.<>p__0.Target((CallSite) <>o__2.<>p__0, base.get_ViewBag(), this.db.appFacade.CanView(str3, "Team"));
                if (<>o__2.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__2.<>p__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanEdit", typeof(TeamController), argumentInfo));
                }
                <>o__2.<>p__1.Target((CallSite) <>o__2.<>p__1, base.get_ViewBag(), this.db.appFacade.CanEdit(str3, "Team"));
                if (<>o__2.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__2.<>p__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__2.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__2.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__2.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__2.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanView", typeof(TeamController), argumentInfo));
                }
                if (<>o__2.<>p__4.Target((CallSite) <>o__2.<>p__4, <>o__2.<>p__3.Target((CallSite) <>o__2.<>p__3, <>o__2.<>p__2.Target((CallSite) <>o__2.<>p__2, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(400);
            }
            object[] objArray1 = new object[] { id };
            OSC_Team team = this.db.Teams.Find(objArray1);
            if (team == null)
            {
                return base.HttpNotFound();
            }
            if (!this.db.IsManaged(new long?(team.TeamId), str2, str))
            {
                return base.HttpNotFound();
            }
            object[] objArray2 = new object[] { team.DepartmentId };
            team.Department = this.db.Departments.Find(objArray2);
            return base.View(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TeamId,TeamName,DepartmentId,IsActive,BIUserGroup,AIQUserGroup")] OSC_Team oSC_Team)
        {
            try
            {
                string str = base.get_Session()["role"].ToString();
                string str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__6.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__6.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanEdit", typeof(TeamController), argumentInfo));
                }
                <>o__6.<>p__0.Target((CallSite) <>o__6.<>p__0, base.get_ViewBag(), this.db.appFacade.CanEdit(str3, "Team"));
                if (<>o__6.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__6.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__6.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__6.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__6.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__6.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanEdit", typeof(TeamController), argumentInfo));
                }
                if (<>o__6.<>p__3.Target((CallSite) <>o__6.<>p__3, <>o__6.<>p__2.Target((CallSite) <>o__6.<>p__2, <>o__6.<>p__1.Target((CallSite) <>o__6.<>p__1, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            if (base.get_Session()["role"].ToString() != "Admin")
            {
                oSC_Team.IsActive = true;
            }
            if (base.get_ModelState().get_IsValid())
            {
                this.db.Entry<OSC_Team>(oSC_Team).set_State(0x10);
                this.db.SaveChanges();
                return base.RedirectToAction("Index");
            }
            return base.View(oSC_Team);
        }

        public ActionResult Edit(long? id)
        {
            try
            {
                string str = base.get_Session()["role"].ToString();
                string str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__5.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__5.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanEdit", typeof(TeamController), argumentInfo));
                }
                <>o__5.<>p__0.Target((CallSite) <>o__5.<>p__0, base.get_ViewBag(), this.db.appFacade.CanEdit(str3, "Team"));
                if (<>o__5.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__5.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__5.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__5.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__5.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__5.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanEdit", typeof(TeamController), argumentInfo));
                }
                if (<>o__5.<>p__3.Target((CallSite) <>o__5.<>p__3, <>o__5.<>p__2.Target((CallSite) <>o__5.<>p__2, <>o__5.<>p__1.Target((CallSite) <>o__5.<>p__1, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(400);
            }
            object[] objArray1 = new object[] { id };
            OSC_Team team = this.db.Teams.Find(objArray1);
            if (team == null)
            {
                return base.HttpNotFound();
            }
            team.GroupIds = (ICollection<OSC_TeamGroupIds>) this.db.GetGroupIds(id);
            if (<>o__5.<>p__4 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__5.<>p__4 = CallSite<Func<CallSite, object, SelectList, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Departments", typeof(TeamController), argumentInfo));
            }
            <>o__5.<>p__4.Target((CallSite) <>o__5.<>p__4, base.get_ViewBag(), new SelectList(this.db.Departments, "DepartmentId", "DepartmentName"));
            return base.View(team);
        }

        public JsonResult GetNptCategories()
        {
            ParameterExpression expression;
            ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
            List<OSC_NptCategory> list = Enumerable.ToList<OSC_NptCategory>((IEnumerable<OSC_NptCategory>) Queryable.Select<OSC_NptCategory, OSC_NptCategory>(this.db.NptCategories, Expression.Lambda<Func<OSC_NptCategory, OSC_NptCategory>>((Expression) (expression = Expression.Parameter(typeof(OSC_NptCategory), "list")), expressionArray1)));
            return base.Json(list, 0);
        }

        public JsonResult GetNptCategories(long? id)
        {
            <>c__DisplayClass18_0 class_;
            ParameterExpression expression;
            if (!id.HasValue)
            {
                return base.Json(null);
            }
            ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
            List<OSC_TeamNptCategory> list = Enumerable.ToList<OSC_TeamNptCategory>((IEnumerable<OSC_TeamNptCategory>) Queryable.Where<OSC_TeamNptCategory>(this.db.TeamNptCategories, Expression.Lambda<Func<OSC_TeamNptCategory, bool>>((Expression) Expression.Equal((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamNptCategory), "list")), (MethodInfo) methodof(OSC_TeamNptCategory.get_TeamId)), (Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass18_0)), fieldof(<>c__DisplayClass18_0.id)), typeof(long))), expressionArray1)));
            return base.Json(list, 0);
        }

        public JsonResult GetTeamGroupIds(long? id)
        {
            <>c__DisplayClass11_0 class_;
            ParameterExpression expression;
            if (!id.HasValue)
            {
                return base.Json(null);
            }
            ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
            List<OSC_TeamGroupIds> list = Enumerable.ToList<OSC_TeamGroupIds>((IEnumerable<OSC_TeamGroupIds>) Queryable.Where<OSC_TeamGroupIds>(this.db.TeamGroupIds, Expression.Lambda<Func<OSC_TeamGroupIds, bool>>((Expression) Expression.Equal((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamGroupIds), "list")), (MethodInfo) methodof(OSC_TeamGroupIds.get_TeamId)), (Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass11_0)), fieldof(<>c__DisplayClass11_0.id)), typeof(long))), expressionArray1)));
            return base.Json(list, 0);
        }

        public JsonResult GetTimings(long? id, int? year)
        {
            if (id.HasValue && year.HasValue)
            {
                int? nullable = year;
                int num = 0;
                if (!((nullable.GetValueOrDefault() == num) ? nullable.HasValue : false) && (year.ToString() != ""))
                {
                    <>c__DisplayClass14_0 class_;
                    ParameterExpression expression;
                    ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
                    List<OSC_TeamWorkItem> list = Enumerable.ToList<OSC_TeamWorkItem>((IEnumerable<OSC_TeamWorkItem>) Queryable.Where<OSC_TeamWorkItem>(this.db.TeamWorkItems, Expression.Lambda<Func<OSC_TeamWorkItem, bool>>((Expression) Expression.AndAlso((Expression) Expression.Equal((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamWorkItem), "list")), (MethodInfo) methodof(OSC_TeamWorkItem.get_TeamId)), (Expression) Expression.Convert((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass14_0)), fieldof(<>c__DisplayClass14_0.id)), typeof(long)), typeof(long?))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) expression, (MethodInfo) methodof(OSC_TeamWorkItem.get_Year)), (Expression) Expression.Convert((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass14_0)), fieldof(<>c__DisplayClass14_0.year)), typeof(int)), typeof(int?)))), expressionArray1)));
                    if ((list == null) || (list.Count == 0))
                    {
                        ParameterExpression[] expressionArray2 = new ParameterExpression[] { expression };
                        list = Enumerable.ToList<OSC_TeamWorkItem>((IEnumerable<OSC_TeamWorkItem>) Queryable.Where<OSC_TeamWorkItem>(this.db.TeamWorkItems, Expression.Lambda<Func<OSC_TeamWorkItem, bool>>((Expression) Expression.AndAlso((Expression) Expression.Equal((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamWorkItem), "list")), (MethodInfo) methodof(OSC_TeamWorkItem.get_TeamId)), (Expression) Expression.Convert((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass14_0)), fieldof(<>c__DisplayClass14_0.id)), typeof(long)), typeof(long?))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) expression, (MethodInfo) methodof(OSC_TeamWorkItem.get_Year)), (Expression) Expression.Convert((Expression) Expression.Subtract((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass14_0)), fieldof(<>c__DisplayClass14_0.year)), typeof(int)), (Expression) Expression.Constant(1, typeof(int))), typeof(int?)))), expressionArray2)));
                    }
                    if ((list == null) || (list.Count == 0))
                    {
                        ParameterExpression[] expressionArray3 = new ParameterExpression[] { expression };
                        list = Enumerable.ToList<OSC_TeamWorkItem>((IEnumerable<OSC_TeamWorkItem>) Queryable.Where<OSC_TeamWorkItem>(this.db.TeamWorkItems, Expression.Lambda<Func<OSC_TeamWorkItem, bool>>((Expression) Expression.AndAlso((Expression) Expression.Equal((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamWorkItem), "list")), (MethodInfo) methodof(OSC_TeamWorkItem.get_TeamId)), (Expression) Expression.Convert((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass14_0)), fieldof(<>c__DisplayClass14_0.id)), typeof(long)), typeof(long?))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) expression, (MethodInfo) methodof(OSC_TeamWorkItem.get_Year)), (Expression) Expression.Convert((Expression) Expression.Add((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass14_0)), fieldof(<>c__DisplayClass14_0.year)), typeof(int)), (Expression) Expression.Constant(1, typeof(int))), typeof(int?)))), expressionArray3)));
                    }
                    return base.Json(list, 0);
                }
            }
            return base.Json(null);
        }

        public PartialViewResult GroupIdSection(long? id)
        {
            if (<>o__10.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__10.<>p__0 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TeamId", typeof(TeamController), argumentInfo));
            }
            <>o__10.<>p__0.Target((CallSite) <>o__10.<>p__0, base.get_ViewBag(), id.Value);
            return base.PartialView();
        }

        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            <>c__DisplayClass1_0 class_;
            string str;
            string str2;
            ParameterExpression expression;
            this.db.InitializeTeams();
            try
            {
                str = base.get_Session()["role"].ToString();
                str2 = base.get_Session()["logon_user"].ToString();
                string str3 = base.get_Session()["grp_id"].ToString();
                if (<>o__1.<>p__0 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__1.<>p__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanView", typeof(TeamController), argumentInfo));
                }
                <>o__1.<>p__0.Target((CallSite) <>o__1.<>p__0, base.get_ViewBag(), this.db.appFacade.CanView(str3, "Team"));
                if (<>o__1.<>p__1 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__1.<>p__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanAdd", typeof(TeamController), argumentInfo));
                }
                <>o__1.<>p__1.Target((CallSite) <>o__1.<>p__1, base.get_ViewBag(), this.db.appFacade.CanAdd(str3, "Team"));
                if (<>o__1.<>p__2 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__1.<>p__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanEdit", typeof(TeamController), argumentInfo));
                }
                <>o__1.<>p__2.Target((CallSite) <>o__1.<>p__2, base.get_ViewBag(), this.db.appFacade.CanEdit(str3, "Team"));
                if (<>o__1.<>p__3 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    <>o__1.<>p__3 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CanDelete", typeof(TeamController), argumentInfo));
                }
                <>o__1.<>p__3.Target((CallSite) <>o__1.<>p__3, base.get_ViewBag(), this.db.appFacade.CanDelete(str3, "Team"));
                if (<>o__1.<>p__6 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__1.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.IsTrue, typeof(TeamController), argumentInfo));
                }
                if (<>o__1.<>p__5 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__1.<>p__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, (ExpressionType) ExpressionType.Not, typeof(TeamController), argumentInfo));
                }
                if (<>o__1.<>p__4 == null)
                {
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
                    <>o__1.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CanView", typeof(TeamController), argumentInfo));
                }
                if (<>o__1.<>p__6.Target((CallSite) <>o__1.<>p__6, <>o__1.<>p__5.Target((CallSite) <>o__1.<>p__5, <>o__1.<>p__4.Target((CallSite) <>o__1.<>p__4, base.get_ViewBag()))))
                {
                    return base.HttpNotFound();
                }
            }
            catch (Exception exception)
            {
                string str4 = exception.Message.ToString();
                return base.HttpNotFound();
            }
            ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
            IQueryable<OSC_Team> queryable = Queryable.Select<OSC_Team, OSC_Team>(this.db.Teams, Expression.Lambda<Func<OSC_Team, OSC_Team>>((Expression) (expression = Expression.Parameter(typeof(OSC_Team), "t")), expressionArray1));
            List<long> TeamIds = new List<long>();
            switch (str)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                case "Staff":
                {
                    foreach (OSC_Team team in queryable)
                    {
                        if (this.db.IsManaged(new long?(team.TeamId), str2, str) && !TeamIds.Contains(team.TeamId))
                        {
                            TeamIds.Add(team.TeamId);
                        }
                    }
                    Expression[] expressionArray2 = new Expression[] { Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_Team), "t")), (MethodInfo) methodof(OSC_Team.get_TeamId)) };
                    ParameterExpression[] expressionArray3 = new ParameterExpression[] { expression };
                    queryable = Queryable.Where<OSC_Team>(this.db.Teams, Expression.Lambda<Func<OSC_Team, bool>>((Expression) Expression.AndAlso((Expression) Expression.Call((Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass1_0)), fieldof(<>c__DisplayClass1_0.TeamIds)), (MethodInfo) methodof(List<long>.Contains, List<long>), expressionArray2), (Expression) Expression.Property((Expression) expression, (MethodInfo) methodof(OSC_Team.get_IsActive))), expressionArray3));
                    break;
                }
            }
            int? nullable = 10;
            if (pageSize.HasValue)
            {
                nullable = pageSize;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                Expression[] expressionArray4 = new Expression[] { Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass1_0)), fieldof(<>c__DisplayClass1_0.searchString)) };
                ParameterExpression[] expressionArray5 = new ParameterExpression[] { expression };
                queryable = Queryable.Where<OSC_Team>(queryable, Expression.Lambda<Func<OSC_Team, bool>>((Expression) Expression.Call((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_Team), "t")), (MethodInfo) methodof(OSC_Team.get_TeamName)), (MethodInfo) methodof(string.Contains), expressionArray4), expressionArray5));
            }
            ParameterExpression[] expressionArray6 = new ParameterExpression[] { expression };
            ParameterExpression[] expressionArray7 = new ParameterExpression[] { expression };
            int? nullable2 = page;
            return base.View(PagedListExtensions.ToPagedList<OSC_Team>((IQueryable<OSC_Team>) Queryable.ThenBy<OSC_Team, string>(Queryable.OrderBy<OSC_Team, long?>(queryable, Expression.Lambda<Func<OSC_Team, long?>>((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_Team), "t")), (MethodInfo) methodof(OSC_Team.get_DepartmentId)), expressionArray6)), Expression.Lambda<Func<OSC_Team, string>>((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_Team), "t")), (MethodInfo) methodof(OSC_Team.get_TeamName)), expressionArray7)), nullable2.HasValue ? nullable2.GetValueOrDefault() : 1, nullable.Value));
        }

        public PartialViewResult NptCategorySection(long? id)
        {
            if (<>o__16.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__16.<>p__0 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TeamId", typeof(TeamController), argumentInfo));
            }
            <>o__16.<>p__0.Target((CallSite) <>o__16.<>p__0, base.get_ViewBag(), id.Value);
            return base.PartialView();
        }

        public JsonResult SaveGroupIds(long? id, List<OSC_TeamGroupIds> objects)
        {
            <>c__DisplayClass12_0 class_;
            ParameterExpression expression;
            var type = new {
                message = "Success"
            };
            if (!id.HasValue)
            {
                type = new {
                    message = "Failed"
                };
                return base.Json(type);
            }
            if (objects == null)
            {
                type = new {
                    message = "Failed"
                };
                return base.Json(type);
            }
            ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
            foreach (OSC_TeamGroupIds obj in Enumerable.ToList<OSC_TeamGroupIds>((IEnumerable<OSC_TeamGroupIds>) Queryable.Where<OSC_TeamGroupIds>(this.db.TeamGroupIds.AsNoTracking(), Expression.Lambda<Func<OSC_TeamGroupIds, bool>>((Expression) Expression.Equal((Expression) Expression.Convert((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamGroupIds), "t")), (MethodInfo) methodof(OSC_TeamGroupIds.get_TeamId)), typeof(long?)), (Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass12_0)), fieldof(<>c__DisplayClass12_0.id))), expressionArray1))))
            {
                if (Enumerable.FirstOrDefault<OSC_TeamGroupIds>((IEnumerable<OSC_TeamGroupIds>) (from t in (IEnumerable<OSC_TeamGroupIds>) objects
                    where (t.TGIId == obj.TGIId) && (obj.TGIId > 0L)
                    select t)) == null)
                {
                    object[] objArray1 = new object[] { obj.TGIId };
                    OSC_TeamGroupIds ids = this.db.TeamGroupIds.Find(objArray1);
                    if (base.get_ModelState().get_IsValid())
                    {
                        this.db.TeamGroupIds.Remove(ids);
                        this.db.SaveChanges();
                    }
                }
            }
            foreach (OSC_TeamGroupIds ids2 in objects)
            {
                if (base.get_ModelState().get_IsValid())
                {
                    if (ids2.TGIId == 0)
                    {
                        this.db.TeamGroupIds.Add(ids2);
                        this.db.SaveChanges();
                    }
                    else
                    {
                        this.db.Entry<OSC_TeamGroupIds>(ids2).set_State(0x10);
                        this.db.SaveChanges();
                    }
                }
            }
            return base.Json(type);
        }

        public JsonResult SaveTimings(long? id, List<OSC_TeamWorkItem> objects)
        {
            <>c__DisplayClass15_0 class_;
            ParameterExpression expression;
            var type = new {
                message = "Success"
            };
            if (!id.HasValue)
            {
                type = new {
                    message = "Failed"
                };
                return base.Json(type);
            }
            if (objects == null)
            {
                type = new {
                    message = "Failed"
                };
                return base.Json(type);
            }
            ParameterExpression[] expressionArray1 = new ParameterExpression[] { expression };
            List<OSC_TeamWorkItem> list = Enumerable.ToList<OSC_TeamWorkItem>((IEnumerable<OSC_TeamWorkItem>) Queryable.Where<OSC_TeamWorkItem>(this.db.TeamWorkItems.AsNoTracking(), Expression.Lambda<Func<OSC_TeamWorkItem, bool>>((Expression) Expression.Equal((Expression) Expression.Property((Expression) (expression = Expression.Parameter(typeof(OSC_TeamWorkItem), "t")), (MethodInfo) methodof(OSC_TeamWorkItem.get_TeamId)), (Expression) Expression.Field((Expression) Expression.Constant(class_, typeof(<>c__DisplayClass15_0)), fieldof(<>c__DisplayClass15_0.id))), expressionArray1)));
            foreach (OSC_TeamWorkItem obj in list)
            {
                if (Enumerable.FirstOrDefault<OSC_TeamWorkItem>((IEnumerable<OSC_TeamWorkItem>) (from t in (IEnumerable<OSC_TeamWorkItem>) objects
                    where (t.WorkItemNo == obj.WorkItemNo) && (obj.WorkItemNo > 0L)
                    select t)) == null)
                {
                    object[] objArray1 = new object[] { obj.WorkItemNo };
                    OSC_TeamWorkItem item = this.db.TeamWorkItems.Find(objArray1);
                    if (base.get_ModelState().get_IsValid())
                    {
                        this.db.TeamWorkItems.Remove(item);
                        this.db.SaveChanges();
                    }
                }
            }
            foreach (OSC_TeamWorkItem item1 in objects)
            {
                if (base.get_ModelState().get_IsValid())
                {
                    if (item1.WorkItemNo == 0)
                    {
                        this.db.TeamWorkItems.Add(item1);
                        this.db.SaveChanges();
                    }
                    else
                    {
                        if (Enumerable.FirstOrDefault<OSC_TeamWorkItem>(Enumerable.Where<OSC_TeamWorkItem>((IEnumerable<OSC_TeamWorkItem>) list, delegate (OSC_TeamWorkItem t) {
                            if (t.WorkItemNo != item1.WorkItemNo)
                            {
                                return false;
                            }
                            int? year = t.Year;
                            int? nullable2 = item1.Year;
                            if (year.GetValueOrDefault() != nullable2.GetValueOrDefault())
                            {
                                return false;
                            }
                            return year.HasValue == nullable2.HasValue;
                        })) != null)
                        {
                            this.db.Entry<OSC_TeamWorkItem>(item1).set_State(0x10);
                            this.db.SaveChanges();
                            continue;
                        }
                        this.db.TeamWorkItems.Add(item1);
                        this.db.SaveChanges();
                    }
                }
            }
            return base.Json(type);
        }

        public ActionResult Settings(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(400);
            }
            object[] objArray1 = new object[] { id };
            OSC_Team team = this.db.Teams.Find(objArray1);
            object[] objArray2 = new object[] { team.DepartmentId };
            team.Department = this.db.Departments.Find(objArray2);
            if (team == null)
            {
                return base.HttpNotFound();
            }
            return base.View(team);
        }

        public PartialViewResult TimingsSection(long? id)
        {
            if (<>o__13.<>p__0 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__13.<>p__0 = CallSite<Func<CallSite, object, DbSet<OSC_BusinessArea>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BusinessAreas", typeof(TeamController), argumentInfo));
            }
            <>o__13.<>p__0.Target((CallSite) <>o__13.<>p__0, base.get_ViewBag(), this.db.BusinessAreas);
            if (<>o__13.<>p__1 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__13.<>p__1 = CallSite<Func<CallSite, object, DbSet<OSC_WorkType>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "WorkTypes", typeof(TeamController), argumentInfo));
            }
            <>o__13.<>p__1.Target((CallSite) <>o__13.<>p__1, base.get_ViewBag(), this.db.WorkTypes);
            if (<>o__13.<>p__2 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__13.<>p__2 = CallSite<Func<CallSite, object, DbSet<OSC_WorkStatus>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Status", typeof(TeamController), argumentInfo));
            }
            <>o__13.<>p__2.Target((CallSite) <>o__13.<>p__2, base.get_ViewBag(), this.db.Statuses);
            if (<>o__13.<>p__3 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__13.<>p__3 = CallSite<Func<CallSite, object, SelectList, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Years", typeof(TeamController), argumentInfo));
            }
            <>o__13.<>p__3.Target((CallSite) <>o__13.<>p__3, base.get_ViewBag(), new SelectList((IEnumerable) this.db.years, "Value", "Text", ""));
            if (<>o__13.<>p__4 == null)
            {
                CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                <>o__13.<>p__4 = CallSite<Func<CallSite, object, long, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TeamId", typeof(TeamController), argumentInfo));
            }
            <>o__13.<>p__4.Target((CallSite) <>o__13.<>p__4, base.get_ViewBag(), id.Value);
            return base.PartialView();
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TeamController.<>c <>9 = new TeamController.<>c();
        }

        [CompilerGenerated]
        private static class <>o__1
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__1;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__3;
            public static CallSite<Func<CallSite, object, object>> <>p__4;
            public static CallSite<Func<CallSite, object, object>> <>p__5;
            public static CallSite<Func<CallSite, object, bool>> <>p__6;
        }

        [CompilerGenerated]
        private static class <>o__10
        {
            public static CallSite<Func<CallSite, object, long, object>> <>p__0;
        }

        [CompilerGenerated]
        private static class <>o__13
        {
            public static CallSite<Func<CallSite, object, DbSet<OSC_BusinessArea>, object>> <>p__0;
            public static CallSite<Func<CallSite, object, DbSet<OSC_WorkType>, object>> <>p__1;
            public static CallSite<Func<CallSite, object, DbSet<OSC_WorkStatus>, object>> <>p__2;
            public static CallSite<Func<CallSite, object, SelectList, object>> <>p__3;
            public static CallSite<Func<CallSite, object, long, object>> <>p__4;
        }

        [CompilerGenerated]
        private static class <>o__16
        {
            public static CallSite<Func<CallSite, object, long, object>> <>p__0;
        }

        [CompilerGenerated]
        private static class <>o__2
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, bool, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, object>> <>p__3;
            public static CallSite<Func<CallSite, object, bool>> <>p__4;
        }

        [CompilerGenerated]
        private static class <>o__3
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, SelectList, object>> <>p__4;
        }

        [CompilerGenerated]
        private static class <>o__4
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__5
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
            public static CallSite<Func<CallSite, object, SelectList, object>> <>p__4;
        }

        [CompilerGenerated]
        private static class <>o__6
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__7
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }

        [CompilerGenerated]
        private static class <>o__8
        {
            public static CallSite<Func<CallSite, object, bool, object>> <>p__0;
            public static CallSite<Func<CallSite, object, object>> <>p__1;
            public static CallSite<Func<CallSite, object, object>> <>p__2;
            public static CallSite<Func<CallSite, object, bool>> <>p__3;
        }
    }
}

