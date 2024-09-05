import MenuComponent from '../core/menu';

const SideBarComponent: React.FC = () => {
    return (
        <div className="sidebar vh-100">
            <h4 className="text-center">Menu</h4>
            <MenuComponent />
        </div>
    );
}

export default SideBarComponent;