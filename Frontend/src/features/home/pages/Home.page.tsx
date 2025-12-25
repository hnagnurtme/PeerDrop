import { useNavigate } from "react-router-dom";
import { nanoid } from "nanoid";



export default function HomePage(){
    const navigate = useNavigate();
    
    const handleCreateRoom = () => {
        const roomId = nanoid(10);
        navigate(`/room/${roomId}`);
    };

    return(
        <div>
            <h2>XIN CHAO</h2>
            <button
                onClick={handleCreateRoom}
                className="blur-0"
            >
                BUTTON
            </button>
        </div>
    )
}