
import React from 'react';

const Footer: React.FC = () => {
  return (
    <footer className="relative z-10 w-full bg-black text-white py-6 border-t-4 border-primary">
      <div className="max-w-7xl mx-auto px-6 flex flex-col md:flex-row justify-between items-center gap-4">
        <div className="font-mono text-xs md:text-sm text-center md:text-left">
          <p>STATUS: OPERATIONAL</p>
          <p className="opacity-60">SERVER_ID: N-WEST-09</p>
        </div>
        
        <div className="flex gap-4">
          {['tw', 'ig', 'li'].map((social) => (
            <a key={social} className="w-10 h-10 border-2 border-white flex items-center justify-center hover:bg-primary hover:border-black hover:text-black transition-colors font-bold" href="#">
              {social}
            </a>
          ))}
        </div>
        
        <div className="font-mono text-xs md:text-sm text-right">
          <p>© 2024 PIXELPORT SYSTEMS</p>
          <a className="underline hover:text-primary transition-colors" href="#">PRIVACY_PROTOCOL.TXT</a>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
