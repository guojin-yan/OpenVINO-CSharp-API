// ============================================
// OpenVINO C# API 文档自定义脚本
// ============================================

// 返回顶部按钮功能
document.addEventListener('DOMContentLoaded', function() {
  // 创建返回顶部按钮
  const backToTopBtn = document.createElement('button');
  backToTopBtn.id = 'back-to-top';
  backToTopBtn.innerHTML = '↑';
  backToTopBtn.title = '返回顶部 / Back to Top';
  document.body.appendChild(backToTopBtn);

  // 滚动时显示/隐藏按钮
  window.addEventListener('scroll', function() {
    if (window.pageYOffset > 300) {
      backToTopBtn.classList.add('visible');
    } else {
      backToTopBtn.classList.remove('visible');
    }
  });

  // 点击返回顶部
  backToTopBtn.addEventListener('click', function() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  });

  // 代码块复制功能
  const codeBlocks = document.querySelectorAll('pre');
  codeBlocks.forEach(function(block) {
    const copyBtn = document.createElement('button');
    copyBtn.className = 'copy-code-btn';
    copyBtn.innerHTML = '📋';
    copyBtn.title = '复制代码 / Copy code';
    copyBtn.style.cssText = `
      position: absolute;
      top: 8px;
      right: 8px;
      background: rgba(255,255,255,0.9);
      border: 1px solid #ddd;
      border-radius: 4px;
      padding: 4px 8px;
      cursor: pointer;
      font-size: 12px;
      opacity: 0;
      transition: opacity 0.2s;
    `;
    
    block.style.position = 'relative';
    block.appendChild(copyBtn);

    block.addEventListener('mouseenter', function() {
      copyBtn.style.opacity = '1';
    });

    block.addEventListener('mouseleave', function() {
      copyBtn.style.opacity = '0';
    });

    copyBtn.addEventListener('click', function() {
      const code = block.querySelector('code')?.textContent || block.textContent;
      navigator.clipboard.writeText(code).then(function() {
        copyBtn.innerHTML = '✓';
        copyBtn.title = '已复制! / Copied!';
        setTimeout(function() {
          copyBtn.innerHTML = '📋';
          copyBtn.title = '复制代码 / Copy code';
        }, 2000);
      });
    });
  });

  // 平滑滚动到锚点
  document.querySelectorAll('a[href^="#"]').forEach(function(anchor) {
    anchor.addEventListener('click', function(e) {
      const targetId = this.getAttribute('href');
      if (targetId !== '#') {
        const targetElement = document.querySelector(targetId);
        if (targetElement) {
          e.preventDefault();
          targetElement.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
          });
        }
      }
    });
  });
});
