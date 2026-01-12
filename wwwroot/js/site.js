// Animations au scroll
document.addEventListener('DOMContentLoaded', function() {
  // Observer pour les animations au scroll
  const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
  };

  const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('animated');
      }
    });
  }, observerOptions);

  // Appliquer aux éléments avec la classe animate-on-scroll
  document.querySelectorAll('.animate-on-scroll').forEach(el => {
    observer.observe(el);
  });

  // Navbar scroll effect
  const navbar = document.querySelector('.navbar');
  let lastScroll = 0;

  window.addEventListener('scroll', function() {
    const currentScroll = window.pageYOffset;

    if (currentScroll > 100) {
      navbar?.classList.add('scrolled');
    } else {
      navbar?.classList.remove('scrolled');
    }

    lastScroll = currentScroll;
  });

  // Smooth scroll pour les ancres
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
      const href = this.getAttribute('href');
      if (href !== '#' && href.length > 1) {
        const target = document.querySelector(href);
        if (target) {
          e.preventDefault();
          target.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
          });
        }
      }
    });
  });

  // Animation des images au hover
  const cards = document.querySelectorAll('.card-hover');
  cards.forEach(card => {
    const img = card.querySelector('img');
    if (img) {
      card.addEventListener('mouseenter', function() {
        img.style.transform = 'scale(1.1)';
      });
      card.addEventListener('mouseleave', function() {
        img.style.transform = 'scale(1)';
      });
    }
  });

  // Parallax effect léger pour le hero
  const hero = document.querySelector('.hero-section');
  if (hero) {
    window.addEventListener('scroll', function() {
      const scrolled = window.pageYOffset;
      const rate = scrolled * 0.5;
      if (scrolled < hero.offsetHeight) {
        hero.style.transform = `translateY(${rate}px)`;
      }
    });
  }

  // Counter animation pour les statistiques
  const statNumbers = document.querySelectorAll('.stat-number');
  
  if (statNumbers.length > 0) {
    const animateCounter = (element) => {
      const originalText = element.getAttribute('data-target') || element.textContent.trim();
      const isPercentage = originalText.includes('%');
      const hasPlus = originalText.includes('+');
      const num = parseInt(originalText.replace(/\D/g, '')) || 0;
      
      if (!num) return;

      let current = 0;
      const increment = Math.max(1, num / 50);
      const timer = setInterval(() => {
        current += increment;
        if (current >= num) {
          element.textContent = originalText;
          clearInterval(timer);
        } else {
          const value = Math.floor(current);
          element.textContent = (hasPlus ? '+' : '') + value + (isPercentage ? '%' : '');
        }
      }, 30);
    };

    const statObserver = new IntersectionObserver(function(entries) {
      entries.forEach(entry => {
        if (entry.isIntersecting && !entry.target.classList.contains('counted')) {
          entry.target.classList.add('counted');
          animateCounter(entry.target);
          statObserver.unobserve(entry.target);
        }
      });
    }, { threshold: 0.5 });

    statNumbers.forEach(stat => {
      const originalText = stat.textContent.trim();
      stat.setAttribute('data-target', originalText);
      stat.textContent = '0';
      statObserver.observe(stat);
    });
  }
});

