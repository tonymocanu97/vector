import SiteFooter from '@/components/site-footer';
import SiteSidebar from '@/components/site-sidebar';

const SiteLayout = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="min-h-screen bg-background">
      <SiteSidebar />
      <div className="flex min-h-screen flex-col lg:pl-64">
        <main className="flex-1">{children}</main>
        <SiteFooter />
      </div>
    </div>
  );
};

export default SiteLayout;
