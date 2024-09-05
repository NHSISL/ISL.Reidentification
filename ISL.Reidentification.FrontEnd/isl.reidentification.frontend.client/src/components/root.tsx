import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import { Outlet } from "react-router-dom";
import NavbarComponent from "./layouts/navbar";
import { useState } from "react";
import SideBarComponent from "./layouts/sidebar";
import FooterComponent from "./layouts/footer";

export default function Root() {
    const [sidebarOpen, setSidebarOpen] = useState(true);

    const toggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
    };

    return (
        <div className="layout-container">
            <div className={`sidebar bg-light ${sidebarOpen ? 'sidebar-open' : 'sidebar-closed'}`}>
                <AuthenticatedTemplate>

                    <SideBarComponent />

                    <div className="footerContent">
                        <FooterComponent />
                    </div>

                </AuthenticatedTemplate>
            </div>

            <div className={`content ${sidebarOpen ? 'content-shift-right' : 'content-shift-left'}`}>
                <NavbarComponent toggleSidebar={toggleSidebar} showMenuButton={true} />

                <div className="content-inner">
                    <AuthenticatedTemplate>
                        <Outlet />
                    </AuthenticatedTemplate>
                    <UnauthenticatedTemplate>
                        <div style={{ marginTop: '-20%' }}>
                            <h2>Please Login</h2>
                        </div>
                    </UnauthenticatedTemplate>
                </div>
            </div>
            
        </div>
    );
}