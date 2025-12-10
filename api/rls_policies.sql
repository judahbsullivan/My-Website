-- Supabase RLS setup for public schema tables exposed via PostgREST.
-- Run this in the Supabase SQL editor or psql against your project.
-- Adjust policies if you want finer-grained access (e.g., only authenticated users instead of public).

-- 1) Enable (and force) RLS on all relevant tables
ALTER TABLE public."__EFMigrationsHistory" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."__EFMigrationsHistory" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetRoles" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetRoles" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetRoleClaims" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetRoleClaims" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserClaims" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserClaims" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUsers" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUsers" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserLogins" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserLogins" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserRoles" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserRoles" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserTokens" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."AspNetUserTokens" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."BlogPosts" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."BlogPosts" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."ContactMessages" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."ContactMessages" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."Projects" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."Projects" FORCE ROW LEVEL SECURITY;
ALTER TABLE public."Playgrounds" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."Playgrounds" FORCE ROW LEVEL SECURITY;

-- 2) Identity & migration tables: lock to service_role only
CREATE POLICY service_role_full__EFMigrationsHistory ON public."__EFMigrationsHistory"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetRoles ON public."AspNetRoles"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetRoleClaims ON public."AspNetRoleClaims"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetUserClaims ON public."AspNetUserClaims"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetUsers ON public."AspNetUsers"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetUserLogins ON public."AspNetUserLogins"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetUserRoles ON public."AspNetUserRoles"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_full_AspNetUserTokens ON public."AspNetUserTokens"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');

-- 3) Public content tables: allow public read, restrict writes to service_role
CREATE POLICY public_read_blogposts ON public."BlogPosts"
  FOR SELECT USING (true);
CREATE POLICY service_role_write_blogposts ON public."BlogPosts"
  FOR INSERT WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_update_blogposts ON public."BlogPosts"
  FOR UPDATE USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_delete_blogposts ON public."BlogPosts"
  FOR DELETE USING ((select auth.role()) = 'service_role');

CREATE POLICY public_read_projects ON public."Projects"
  FOR SELECT USING (true);
CREATE POLICY service_role_write_projects ON public."Projects"
  FOR INSERT WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_update_projects ON public."Projects"
  FOR UPDATE USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_delete_projects ON public."Projects"
  FOR DELETE USING ((select auth.role()) = 'service_role');

CREATE POLICY public_read_playgrounds ON public."Playgrounds"
  FOR SELECT USING (true);
CREATE POLICY service_role_write_playgrounds ON public."Playgrounds"
  FOR INSERT WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_update_playgrounds ON public."Playgrounds"
  FOR UPDATE USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');
CREATE POLICY service_role_delete_playgrounds ON public."Playgrounds"
  FOR DELETE USING ((select auth.role()) = 'service_role');

-- 4) Sensitive tables (contact messages): restrict everything to service_role
CREATE POLICY service_role_full_contactmessages ON public."ContactMessages"
  FOR ALL USING ((select auth.role()) = 'service_role') WITH CHECK ((select auth.role()) = 'service_role');

-- Notes:
-- - If you prefer authenticated-only instead of public read, replace `USING (true)` with `USING (auth.role() = 'authenticated')`.
-- - If any tables shouldn’t be exposed via PostgREST at all, remove them from the publication or move them to a private schema instead of adding broad policies.
